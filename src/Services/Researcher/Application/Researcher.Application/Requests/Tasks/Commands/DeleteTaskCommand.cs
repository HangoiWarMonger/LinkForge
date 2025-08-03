using Ardalis.GuardClauses;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Tasks.Commands;

/// <summary>
/// Команда удаления задачи.
/// </summary>
/// <param name="TaskItemId">Идентификатор задачи.</param>
public record DeleteTaskCommand(Guid TaskItemId);

/// <summary>
/// Обработчик команды удаления задачи.
/// </summary>
public class DeleteTaskCommandHandler
{
    private readonly IRepository<TaskItem> _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskCommandHandler(
        IRepository<TaskItem> taskRepository,
        IUnitOfWork unitOfWork)
    {
        _taskRepository = Guard.Against.Null(taskRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
    }

    /// <summary>
    /// Удаляет задачу по идентификатору.
    /// </summary>
    /// <param name="request">Команда с идентификатором задачи.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача удаления.</returns>
    /// <exception cref="ArgumentException">Если TaskItemId — значение по умолчанию.</exception>
    /// <exception cref="InvalidOperationException">Если задача не найдена.</exception>
    public async Task Handle(
        DeleteTaskCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.TaskItemId);

        var task = await _taskRepository.GetByIdAsync(
            request.TaskItemId,
            trackChanges: true,
            cancellationToken);

        ThrowIf.EntityIsNull(task, request.TaskItemId);

        await _taskRepository.RemoveAsync(task, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}