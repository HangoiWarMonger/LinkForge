using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Graphs.Edges.Commands;

/// <summary>
/// Команда создания нового ребра между двумя узлами.
/// </summary>
/// <param name="FromNodeId">Идентификатор узла-источника.</param>
/// <param name="ToNodeId">Идентификатор узла-приёмника.</param>
/// <param name="Type">Тип ребра.</param>
/// <param name="Description">Описание ребра (опционально).</param>
public record CreateEdgeCommand(
    Guid FromNodeId,
    Guid ToNodeId,
    string Type,
    string? Description
);

/// <summary>
/// Обработчик команды создания ребра.
/// </summary>
public class CreateEdgeCommandHandler
{
    private readonly IRepository<Edge> _edgeRepository;
    private readonly IRepository<Node> _nodeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateEdgeCommandHandler(
        IRepository<Edge> edgeRepository,
        IRepository<Node> nodeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _edgeRepository = Guard.Against.Null(edgeRepository);
        _nodeRepository = Guard.Against.Null(nodeRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Создаёт новое ребро и возвращает его DTO.
    /// </summary>
    /// <param name="request">Команда с параметрами ребра.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO созданного ребра.</returns>
    /// <exception cref="ArgumentException">Если FromNodeId или ToNodeId — значение по умолчанию, или Type пустой.</exception>
    /// <exception cref="InvalidOperationException">Если узлы не найдены.</exception>
    public async Task<EdgeDto> Handle(
        CreateEdgeCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.FromNodeId);
        Guard.Against.Default(request.ToNodeId);
        Guard.Against.NullOrWhiteSpace(request.Type);

        var fromNode = await _nodeRepository.GetByIdAsync(
            request.FromNodeId,
            trackChanges: false,
            cancellationToken);
        ThrowIf.EntityIsNull(fromNode, request.FromNodeId);

        var toNode = await _nodeRepository.GetByIdAsync(
            request.ToNodeId,
            trackChanges: false,
            cancellationToken);
        ThrowIf.EntityIsNull(toNode, request.ToNodeId);

        var edge = new Edge(
            Guid.NewGuid(),
            request.Type,
            request.Description,
            fromNode,
            toNode);

        await _edgeRepository.AddAsync(edge, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EdgeDto>(edge);
    }
}