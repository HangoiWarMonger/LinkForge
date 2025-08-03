using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;

namespace Researcher.Application.Requests.Graphs.Edges.Queries;

/// <summary>
/// Запрос списка ребер по идентификатору графа.
/// </summary>
/// <param name="GraphId">Идентификатор графа.</param>
public record ListEdgesByGraphQuery(Guid GraphId);

/// <summary>
/// Обработчик запроса списка ребер графа.
/// </summary>
public class ListEdgesByGraphQueryHandler
{
    private readonly IRepository<Edge> _edgeRepository;
    private readonly IMapper _mapper;

    public ListEdgesByGraphQueryHandler(
        IRepository<Edge> edgeRepository,
        IMapper mapper)
    {
        _edgeRepository = Guard.Against.Null(edgeRepository);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Возвращает список ребер для заданного графа.
    /// </summary>
    /// <param name="request">Запрос с идентификатором графа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список DTO ребер.</returns>
    /// <exception cref="ArgumentException">Если GraphId — значение по умолчанию.</exception>
    public async Task<IReadOnlyList<EdgeDto>> Handle(
        ListEdgesByGraphQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.GraphId);

        // Предполагается, что все ребра принадлежат узлам графа,
        // поэтому фильтруем по GraphId узла-источника.
        var edges = await _edgeRepository.QueryAsync(
            e => e.FromNode.GraphId == request.GraphId,
            trackChanges: false,
            cancellationToken: cancellationToken);

        return _mapper.Map<List<EdgeDto>>(edges);
    }
}