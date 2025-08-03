using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Tasks.Commands;

/// <summary>
/// Команда перемещения задачи между родителями.
/// </summary>
/// <param name="TaskItemId">Идентификатор задачи.</param>
/// <param name="NewParentId">Идентификатор нового родителя (или null для корня).</param>
public record MoveTaskCommand(Guid TaskItemId, Guid? NewParentId);

/// <summary>
/// Обработчик команды перемещения задачи.
/// </summary>
public class MoveTaskCommandHandler
{
    private readonly IRepository<TaskItem> _taskRepository;
    private readonly IRepository<TaskItem> _parentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MoveTaskCommandHandler(
        IRepository<TaskItem> taskRepository,
        IRepository<TaskItem> parentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _taskRepository = Guard.Against.Null(taskRepository);
        _parentRepository = Guard.Against.Null(parentRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Перемещает задачу под нового родителя.
    /// </summary>
    /// <param name="request">Команда с идентификатором задачи и нового родителя.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновлённый DTO задачи.</returns>
    /// <exception cref="ArgumentException">Если TaskItemId — значение по умолчанию.</exception>
    /// <exception cref="InvalidOperationException">Если задача или новый родитель не найдены.</exception>
    public async Task<TaskItemDto> Handle(
        MoveTaskCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.TaskItemId);

        var task = await _taskRepository.GetByIdAsync(
            request.TaskItemId,
            trackChanges: true,
            cancellationToken);
        ThrowIf.EntityIsNull(task, request.TaskItemId);

        TaskItem? newParent = null;
        if (request.NewParentId.HasValue)
        {
            newParent = await _parentRepository.GetByIdAsync(
                request.NewParentId.Value,
                trackChanges: false,
                cancellationToken);
            ThrowIf.EntityIsNull(newParent, request.NewParentId.Value);
        }

        task.Update(
            task.Title,
            task.Description,
            task.Status,
            newParent);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TaskItemDto>(task);
    }
}