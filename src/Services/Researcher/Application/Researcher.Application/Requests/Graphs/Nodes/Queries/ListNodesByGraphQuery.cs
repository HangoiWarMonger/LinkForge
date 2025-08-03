using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;

namespace Researcher.Application.Requests.Graphs.Nodes.Queries;

/// <summary>
/// Запрос списка узлов по идентификатору графа.
/// </summary>
/// <param name="GraphId">Идентификатор графа.</param>
public record ListNodesByGraphQuery(Guid GraphId);

/// <summary>
/// Обработчик запроса списка узлов графа.
/// </summary>
public class ListNodesByGraphQueryHandler
{
    private readonly IRepository<Node> _nodeRepository;
    private readonly IMapper _mapper;

    public ListNodesByGraphQueryHandler(
        IRepository<Node> nodeRepository,
        IMapper mapper)
    {
        _nodeRepository = Guard.Against.Null(nodeRepository);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Возвращает список узлов для заданного графа.
    /// </summary>
    /// <param name="request">Запрос с идентификатором графа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список DTO узлов.</returns>
    /// <exception cref="ArgumentException">Если GraphId — значение по умолчанию.</exception>
    public async Task<IReadOnlyList<NodeDto>> Handle(
        ListNodesByGraphQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.GraphId);

        var nodes = await _nodeRepository.QueryAsync(
            n => n.GraphId == request.GraphId,
            trackChanges: false,
            cancellationToken: cancellationToken);

        return _mapper.Map<List<NodeDto>>(nodes);
    }
}