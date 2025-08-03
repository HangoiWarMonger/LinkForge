using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Tasks.Queries;

/// <summary>
/// Запрос деталей задачи по идентификатору.
/// </summary>
/// <param name="TaskItemId">Идентификатор задачи.</param>
public record GetTaskByIdQuery(Guid TaskItemId);

/// <summary>
/// Обработчик запроса получения задачи по Id.
/// </summary>
public class GetTaskByIdQueryHandler
{
    private readonly IRepository<TaskItem> _taskRepository;
    private readonly IMapper _mapper;

    public GetTaskByIdQueryHandler(
        IRepository<TaskItem> taskRepository,
        IMapper mapper)
    {
        _taskRepository = Guard.Against.Null(taskRepository);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Получает DTO задачи по идентификатору.
    /// </summary>
    /// <param name="request">Запрос с Id задачи.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO задачи.</returns>
    /// <exception cref="ArgumentException">Если TaskItemId — значение по умолчанию.</exception>
    /// <exception cref="InvalidOperationException">Если задача не найдена.</exception>
    public async Task<TaskItemDto> Handle(
        GetTaskByIdQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.TaskItemId);

        var task = await _taskRepository.GetByIdAsync(
            request.TaskItemId,
            trackChanges: false,
            cancellationToken);

        ThrowIf.EntityIsNull(task, request.TaskItemId);

        return _mapper.Map<TaskItemDto>(task);
    }
}