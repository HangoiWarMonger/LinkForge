using System.Linq.Expressions;
using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Application.Common.Models;
using Researcher.Application.Common.Services;
using Researcher.Domain.Entities;

namespace Researcher.Application.Requests.Projects.Queries;

/// <summary>
/// Запрос списка проектов с пагинацией и сортировкой.
/// </summary>
/// <param name="OrderBy">Имя свойства для сортировки, либо null/пустое для отсутствия сортировки.</param>
/// <param name="SortDirection">Направление сортировки (по умолчанию Ascending).</param>
/// <param name="Page">Номер страницы (начинается с 1), либо null для отключения пагинации.</param>
/// <param name="PageSize">Размер страницы, либо null для отключения пагинации.</param>
public record ListProjectsQuery(
    string? OrderBy = null,
    SortDirection SortDirection = SortDirection.Ascending,
    int? Page = null,
    int? PageSize = null
);

/// <summary>
/// Обработчик запроса списка проектов.
/// </summary>
public class ListProjectsQueryHandler
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IMapper _mapper;

    public ListProjectsQueryHandler(
        IRepository<Project> projectRepository,
        IMapper mapper)
    {
        _projectRepository = Guard.Against.Null(projectRepository);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Возвращает пагинированный список проектов.
    /// </summary>
    /// <param name="request">Запрос (параметры отсутствуют).</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Пагинированный результат с DTO проектов.</returns>
    public async Task<PaginatedResult<ProjectDto>> Handle(
        ListProjectsQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.NegativeOrZero(request.Page ?? 1);
        Guard.Against.NegativeOrZero(request.PageSize ?? 1);
        if (request.OrderBy is not null)
            Guard.Against.NullOrWhiteSpace(request.OrderBy);

        Expression<Func<Project, object>>? orderByExpr = null;
        var ascending = true;

        if (!string.IsNullOrWhiteSpace(request.OrderBy))
        {
            orderByExpr = ReflectionHelper.GetPropertyExpression<Project>(request.OrderBy!);
            ascending = request.SortDirection == SortDirection.Ascending;
        }

        var projects = await _projectRepository.QueryAsync(
            predicate: null,
            orderBy: orderByExpr,
            ascending: ascending,
            page: request.Page,
            pageSize: request.PageSize,
            trackChanges: false,
            cancellationToken: cancellationToken);

        return _mapper.Map<PaginatedResult<ProjectDto>>(projects);
    }
}