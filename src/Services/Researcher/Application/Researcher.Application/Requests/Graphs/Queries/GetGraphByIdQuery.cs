using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Graphs.Queries;

/// <summary>
/// Запрос деталей графа по идентификатору.
/// </summary>
/// <param name="GraphId">Идентификатор графа.</param>
public record GetGraphByIdQuery(Guid GraphId);

/// <summary>
/// Обработчик запроса получения графа по Id.
/// </summary>
public class GetGraphByIdQueryHandler
{
    private readonly IRepository<Graph> _graphRepository;
    private readonly IMapper _mapper;

    public GetGraphByIdQueryHandler(
        IRepository<Graph> graphRepository,
        IMapper mapper)
    {
        _graphRepository = Guard.Against.Null(graphRepository);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Получает DTO графа по идентификатору.
    /// </summary>
    /// <param name="request">Запрос с Id графа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO графа.</returns>
    /// <exception cref="ArgumentException">Если GraphId — значение по умолчанию.</exception>
    /// <exception cref="InvalidOperationException">Если граф не найден.</exception>
    public async Task<GraphDto> Handle(
        GetGraphByIdQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.GraphId);

        var graph = await _graphRepository.GetByIdAsync(
            request.GraphId,
            trackChanges: false,
            cancellationToken);

        ThrowIf.EntityIsNull(graph, request.GraphId);

        return _mapper.Map<GraphDto>(graph);
    }
}