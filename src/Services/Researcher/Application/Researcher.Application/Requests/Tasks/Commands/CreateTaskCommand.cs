using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;
using Researcher.Domain.ValueObjects;

namespace Researcher.Application.Requests.Tasks.Commands;

/// <summary>
/// Команда создания задачи, включая вложенную.
/// </summary>
/// <param name="ProjectId">Идентификатор проекта.</param>
/// <param name="Title">Заголовок задачи.</param>
/// <param name="Status">Статус задачи.</param>
/// <param name="ParentId">Идентификатор родительской задачи (опционально).</param>
/// <param name="Description">Описание задачи (опционально).</param>
public record CreateTaskCommand(
    Guid ProjectId,
    string Title,
    TaskItemStatus Status,
    Guid? ParentId,
    string? Description
);

/// <summary>
/// Обработчик команды создания задачи.
/// </summary>
public class CreateTaskCommandHandler
{
    private readonly IRepository<TaskItem> _taskRepository;
    private readonly IRepository<TaskItem> _parentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTaskCommandHandler(
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
    /// Создаёт новую задачу и возвращает её DTO.
    /// </summary>
    /// <param name="request">Команда с параметрами задачи.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO созданной задачи.</returns>
    /// <exception cref="ArgumentException">Если ProjectId — значение по умолчанию, Title пустой или Status вне диапазона.</exception>
    /// <exception cref="InvalidOperationException">Если родительская задача не найдена (если указан ParentId).</exception>
    public async Task<TaskItemDto> Handle(
        CreateTaskCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.ProjectId);
        Guard.Against.NullOrWhiteSpace(request.Title);
        Guard.Against.EnumOutOfRange(request.Status);

        TaskItem? parent = null;
        if (request.ParentId.HasValue)
        {
            parent = await _parentRepository.GetByIdAsync(
                request.ParentId.Value,
                trackChanges: false,
                cancellationToken);
            ThrowIf.EntityIsNull(parent, request.ParentId.Value);
        }

        var task = new TaskItem(
            Guid.NewGuid(),
            request.Title,
            request.Description,
            request.Status,
            request.ProjectId,
            parent
        );

        await _taskRepository.AddAsync(task, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TaskItemDto>(task);
    }
}