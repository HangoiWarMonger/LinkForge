using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;
using Researcher.Domain.ValueObjects;

namespace Researcher.Application.Requests.Tasks.Commands;

/// <summary>
/// Команда смены статуса задачи.
/// </summary>
/// <param name="TaskItemId">Идентификатор задачи.</param>
/// <param name="NewStatus">Новый статус задачи.</param>
public record ChangeTaskStatusCommand(Guid TaskItemId, TaskItemStatus NewStatus);

/// <summary>
/// Обработчик команды смены статуса задачи.
/// </summary>
public class ChangeTaskStatusCommandHandler
{
    private readonly IRepository<TaskItem> _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ChangeTaskStatusCommandHandler(
        IRepository<TaskItem> taskRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _taskRepository = Guard.Against.Null(taskRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Меняет статус задачи и возвращает обновлённый DTO.
    /// </summary>
    /// <param name="request">Команда с идентификатором задачи и новым статусом.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновлённый DTO задачи.</returns>
    /// <exception cref="ArgumentException">Если TaskItemId — значение по умолчанию или статус вне диапазона.</exception>
    /// <exception cref="InvalidOperationException">Если задача не найдена.</exception>
    public async Task<TaskItemDto> Handle(
        ChangeTaskStatusCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.TaskItemId);
        Guard.Against.EnumOutOfRange(request.NewStatus);

        var task = await _taskRepository.GetByIdAsync(
            request.TaskItemId,
            trackChanges: true,
            cancellationToken);

        ThrowIf.EntityIsNull(task, request.TaskItemId);

        task.Update(
            task.Title,
            task.Description,
            request.NewStatus,
            task.Parent);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TaskItemDto>(task);
    }
}