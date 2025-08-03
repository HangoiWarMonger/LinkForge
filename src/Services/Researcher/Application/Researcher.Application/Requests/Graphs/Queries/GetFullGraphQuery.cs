using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Graphs.Queries;

/// <summary>
/// Запрос полного графа с метаданными, узлами и связями.
/// </summary>
/// <param name="GraphId">Идентификатор графа.</param>
public record GetFullGraphQuery(Guid GraphId);

/// <summary>
/// Обработчик запроса полного графа.
/// </summary>
public class GetFullGraphQueryHandler
{
    private readonly IRepository<Graph> _graphRepo;
    private readonly IMapper _mapper;

    public GetFullGraphQueryHandler(
        IRepository<Graph> graphRepo,
        IMapper mapper)
    {
        _graphRepo = Guard.Against.Null(graphRepo);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Получает полный граф с жадной загрузкой узлов и связей, и маппит в DTO.
    /// </summary>
    /// <param name="request">Запрос с идентификатором графа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Полный DTO графа.</returns>
    /// <exception cref="ArgumentException">Если GraphId — значение по умолчанию.</exception>
    /// <exception cref="InvalidOperationException">Если граф не найден.</exception>
    public async Task<FullGraphDto> Handle(
        GetFullGraphQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.GraphId);

        var graph = await _graphRepo.GetByIdAsync(
            request.GraphId,
            trackChanges: false,
            cancellationToken,
            g => g.Nodes,
            g => g.Edges);

        ThrowIf.EntityIsNull(graph, request.GraphId);

        return _mapper.Map<FullGraphDto>(graph);
    }
}