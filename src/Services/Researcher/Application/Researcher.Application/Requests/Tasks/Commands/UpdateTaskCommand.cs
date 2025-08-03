using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;
using Researcher.Domain.ValueObjects;

namespace Researcher.Application.Requests.Tasks.Commands;

/// <summary>
/// Команда полного обновления задачи (включая смену родителя и статуса).
/// </summary>
/// <param name="TaskItemId">Идентификатор задачи.</param>
/// <param name="NewTitle">Новое название задачи.</param>
/// <param name="NewStatus">Новый статус задачи.</param>
/// <param name="NewParentId">Идентификатор нового родителя (опционально).</param>
/// <param name="NewDescription">Новое описание задачи (опционально).</param>
public record UpdateTaskCommand(
    Guid TaskItemId,
    string NewTitle,
    TaskItemStatus NewStatus,
    Guid? NewParentId,
    string? NewDescription
);

/// <summary>
/// Обработчик команды полного обновления задачи.
/// </summary>
public class UpdateTaskCommandHandler
{
    private readonly IRepository<TaskItem> _taskRepository;
    private readonly IRepository<TaskItem> _parentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTaskCommandHandler(
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
    /// Обновляет задачу и возвращает актуальный DTO.
    /// </summary>
    /// <param name="request">Команда с новыми параметрами задачи.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO обновлённой задачи.</returns>
    /// <exception cref="ArgumentException">Если TaskItemId — значение по умолчанию, NewTitle пустой или NewStatus вне диапазона.</exception>
    /// <exception cref="InvalidOperationException">Если задача или новый родитель не найдены.</exception>
    public async Task<TaskItemDto> Handle(
        UpdateTaskCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.TaskItemId);
        Guard.Against.NullOrWhiteSpace(request.NewTitle);
        Guard.Against.EnumOutOfRange(request.NewStatus);

        var task = await _taskRepository.GetByIdAsync(
            request.TaskItemId,
            trackChanges: true,
            cancellationToken);

        ThrowIf.EntityIsNull(task, request.TaskItemId);

        TaskItem? parent = null;
        if (request.NewParentId.HasValue)
        {
            parent = await _parentRepository.GetByIdAsync(
                request.NewParentId.Value,
                trackChanges: false,
                cancellationToken);
            ThrowIf.EntityIsNull(parent, request.NewParentId.Value);
        }

        task.Update(
            request.NewTitle,
            request.NewDescription,
            request.NewStatus,
            parent);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TaskItemDto>(task);
    }
}