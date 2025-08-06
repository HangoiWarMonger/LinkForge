using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;

namespace Researcher.Application.Requests.Tasks.Queries;

/// <summary>
/// Запрос: все задачи проекта (плоский список с ParentId для восстановления дерева).
/// </summary>
/// <param name="ProjectId">Идентификатор проекта.</param>
public record GetTaskTreeByProjectQuery(Guid ProjectId);

/// <summary>
/// Обработчик запроса на получение списка задач проекта.
/// </summary>
public class GetTaskTreeByProjectQueryHandler
{
    private readonly IRepository<TaskItem> _taskRepository;
    private readonly IMapper _mapper;

    public GetTaskTreeByProjectQueryHandler(
        IRepository<TaskItem> taskRepository,
        IMapper mapper)
    {
        _taskRepository = Guard.Against.Null(taskRepository);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Возвращает плоский список задач проекта.
    /// </summary>
    /// <param name="request">Запрос с идентификатором проекта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список DTO задач.</returns>
    /// <exception cref="ArgumentException">Если ProjectId — значение по умолчанию.</exception>
    public async Task<IReadOnlyList<TaskItemDto>> Handle(
        GetTaskTreeByProjectQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.ProjectId);

        var tasks = await _taskRepository.QueryAsync(
            t => t.ProjectId == request.ProjectId,
            trackChanges: false,
            cancellationToken: cancellationToken);

        return _mapper.Map<List<TaskItemDto>>(tasks.Items);
    }
}