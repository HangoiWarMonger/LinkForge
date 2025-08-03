using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;
using Researcher.Domain.ValueObjects;

namespace Researcher.Application.Requests.Graphs.Commands;

/// <summary>
/// Команда массового обновления графа: метаданные, узлы и ребра.
/// </summary>
/// <param name="GraphId">Идентификатор графа.</param>
/// <param name="NewTitle">Новое название графа.</param>
/// <param name="NewDescription">Новое описание графа (опционально).</param>
/// <param name="Nodes">Список узлов для обновления.</param>
/// <param name="Edges">Список ребер для обновления.</param>
public record UpdateFullGraphCommand(
    Guid GraphId,
    string NewTitle,
    string? NewDescription,
    IReadOnlyList<NodeDto> Nodes,
    IReadOnlyList<EdgeDto> Edges
);

/// <summary>
/// Обработчик команды массового обновления графа.
/// </summary>
public class UpdateFullGraphCommandHandler
{
    private readonly IRepository<Graph> _graphRepo;
    private readonly IRepository<Node> _nodeRepo;
    private readonly IRepository<Edge> _edgeRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateFullGraphCommandHandler(
        IRepository<Graph> graphRepo,
        IRepository<Node> nodeRepo,
        IRepository<Edge> edgeRepo,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _graphRepo = Guard.Against.Null(graphRepo);
        _nodeRepo = Guard.Against.Null(nodeRepo);
        _edgeRepo = Guard.Against.Null(edgeRepo);
        _unitOfWork = Guard.Against.Null(unitOfWork);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Выполняет массовое обновление графа с синхронизацией узлов и ребер.
    /// </summary>
    /// <param name="request">Команда с данными для обновления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновлённый полный DTO графа.</returns>
    /// <exception cref="ArgumentException">Если GraphId — значение по умолчанию или NewTitle пустой.</exception>
    /// <exception cref="InvalidOperationException">Если граф, узлы или ребра не найдены.</exception>
    public async Task<FullGraphDto> Handle(
        UpdateFullGraphCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.GraphId);
        Guard.Against.NullOrWhiteSpace(request.NewTitle);

        // 1. Жадно загружаем граф с текущими узлами и ребрами
        var graph = await _graphRepo.GetByIdAsync(
            request.GraphId,
            trackChanges: true,
            cancellationToken,
            g => g.Nodes,
            g => g.Edges);

        ThrowIf.EntityIsNull(graph, request.GraphId);

        // 2. Обновляем метаданные графа
        graph.Update(request.NewTitle, request.NewDescription, graph.ProjectId);

        // 3. Синхронизация узлов: обновление, добавление, удаление
        var existingNodes = graph.Nodes.ToList();

        foreach (var ndto in request.Nodes)
        {
            var pos = new Position(ndto.Position.X, ndto.Position.Y);
            var node = existingNodes.FirstOrDefault(n => n.Id == ndto.Id);

            if (node is not null)
            {
                node.Update(ndto.Title, ndto.Description, ndto.Type, pos, graph.Id);
                existingNodes.Remove(node);
            }
            else
            {
                var newNode = new Node(Guid.NewGuid(), ndto.Title, ndto.Description, ndto.Type, pos, graph.Id);
                await _nodeRepo.AddAsync(newNode, cancellationToken);
            }
        }

        // Удаляем оставшиеся устаревшие узлы
        foreach (var stale in existingNodes)
        {
            await _nodeRepo.RemoveAsync(stale, cancellationToken);
        }

        // 4. Синхронизация ребер: обновление, добавление, удаление
        var existingEdges = graph.Edges.ToList();

        foreach (var edto in request.Edges)
        {
            var from = await _nodeRepo.GetByIdAsync(edto.FromNodeId, false, cancellationToken);
            var to = await _nodeRepo.GetByIdAsync(edto.ToNodeId, false, cancellationToken);

            ThrowIf.EntityIsNull(from, edto.FromNodeId);
            ThrowIf.EntityIsNull(to, edto.ToNodeId);

            var edge = existingEdges.FirstOrDefault(e => e.Id == edto.Id);

            if (edge is not null)
            {
                edge.Update(edto.Type, edto.Description, from, to);
                existingEdges.Remove(edge);
            }
            else
            {
                var newEdge = new Edge(Guid.NewGuid(), edto.Type, edto.Description, from, to);
                await _edgeRepo.AddAsync(newEdge, cancellationToken);
            }
        }

        // Удаляем устаревшие ребра
        foreach (var stale in existingEdges)
        {
            await _edgeRepo.RemoveAsync(stale, cancellationToken);
        }

        // 5. Сохраняем все изменения в одной транзакции
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 6. Возвращаем обновлённый полный граф с вложениями
        return _mapper.Map<FullGraphDto>(graph);
    }
}