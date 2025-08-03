using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Application.Common.Models;
using Researcher.Domain.Entities;

namespace Researcher.Application.Requests.Graphs.Queries;

/// <summary>
/// Запрос списка графов по идентификатору проекта с пагинацией.
/// </summary>
/// <param name="ProjectId">Идентификатор проекта.</param>
/// <param name="Page">Номер страницы (начинается с 1), либо null для отключения пагинации.</param>
/// <param name="PageSize">Размер страницы, либо null для отключения пагинации.</param>
public record ListGraphsByProjectQuery(
    Guid ProjectId,
    int? Page = null,
    int? PageSize = null
);

/// <summary>
/// Обработчик запроса списка графов проекта с пагинацией.
/// </summary>
public class ListGraphsByProjectQueryHandler
{
    private readonly IRepository<Graph> _graphRepository;
    private readonly IMapper _mapper;

    public ListGraphsByProjectQueryHandler(
        IRepository<Graph> graphRepository,
        IMapper mapper)
    {
        _graphRepository = Guard.Against.Null(graphRepository);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Возвращает пагинированный список графов проекта.
    /// </summary>
    /// <param name="request">Параметры запроса.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Пагинированный результат DTO графов.</returns>
    /// <exception cref="ArgumentException">Если ProjectId — значение по умолчанию.</exception>
    public async Task<PaginatedResult<GraphDto>> Handle(
        ListGraphsByProjectQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.ProjectId);
        Guard.Against.NegativeOrZero(request.Page ?? 1);
        Guard.Against.NegativeOrZero(request.PageSize ?? 1);

        var graphs = await _graphRepository.QueryAsync(
            predicate: g => g.ProjectId == request.ProjectId,
            orderBy: null, // Сортировка не нужна
            ascending: true,
            page: request.Page,
            pageSize: request.PageSize,
            trackChanges: false,
            cancellationToken: cancellationToken);

        return _mapper.Map<PaginatedResult<GraphDto>>(graphs);
    }
}