using System.Linq.Expressions;
using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Application.Common.Models;
using Researcher.Application.Common.Services;
using Researcher.Domain.Entities;
using Researcher.Domain.ValueObjects;

namespace Researcher.Application.Requests.Tasks.Queries;

/// <summary>
/// Запрос списка задач проекта с фильтрацией по статусу, пагинацией и сортировкой.
/// </summary>
/// <param name="ProjectId">Идентификатор проекта.</param>
/// <param name="Status">Опциональный фильтр по статусу задачи.</param>
/// <param name="OrderBy">Имя свойства для сортировки (например, "Title"), либо null/пустое для отсутствия сортировки.</param>
/// <param name="SortDirection">Направление сортировки (по умолчанию Ascending).</param>
/// <param name="Page">Номер страницы (начинается с 1), либо null для отключения пагинации.</param>
/// <param name="PageSize">Размер страницы, либо null для отключения пагинации.</param>
public record ListTasksByProjectQuery(
    Guid ProjectId,
    TaskItemStatus? Status,
    string? OrderBy = null,
    SortDirection SortDirection = SortDirection.Ascending,
    int? Page = null,
    int? PageSize = null
);

/// <summary>
/// Обработчик запроса списка задач проекта с фильтрацией.
/// </summary>
public class ListTasksByProjectQueryHandler
{
    private readonly IRepository<TaskItem> _taskRepository;
    private readonly IMapper _mapper;

    public ListTasksByProjectQueryHandler(
        IRepository<TaskItem> taskRepository,
        IMapper mapper)
    {
        _taskRepository = Guard.Against.Null(taskRepository);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Получает пагинированный список задач проекта с опциональным фильтром по статусу.
    /// </summary>
    /// <param name="request">Запрос с параметрами фильтрации.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Пагинированный результат DTO задач.</returns>
    /// <exception cref="ArgumentException">Если ProjectId — значение по умолчанию.</exception>
    public async Task<PaginatedResult<TaskItemDto>> Handle(
        ListTasksByProjectQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.ProjectId);
        Guard.Against.NegativeOrZero(request.Page ?? 1);
        Guard.Against.NegativeOrZero(request.PageSize ?? 1);

        if (request.OrderBy is not null)
            Guard.Against.NullOrWhiteSpace(request.OrderBy);

        Expression<Func<TaskItem, object>>? orderByExpr = null;
        var ascending = true;

        if (!string.IsNullOrWhiteSpace(request.OrderBy))
        {
            orderByExpr = ReflectionHelper.GetPropertyExpression<TaskItem>(request.OrderBy!);
            ascending = request.SortDirection == SortDirection.Ascending;
        }

        var pagedTasks = await _taskRepository.QueryAsync(
            predicate: t => t.ProjectId == request.ProjectId
                            && (request.Status == null || t.Status == request.Status.Value),
            orderBy: orderByExpr,
            ascending: ascending,
            page: request.Page,
            pageSize: request.PageSize,
            trackChanges: false,
            cancellationToken: cancellationToken);

        return _mapper.Map<PaginatedResult<TaskItemDto>>(pagedTasks);
    }
}