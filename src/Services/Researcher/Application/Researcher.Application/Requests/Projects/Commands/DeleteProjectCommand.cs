using Ardalis.GuardClauses;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Projects.Commands;

/// <summary>
/// Команда удаления проекта.
/// </summary>
/// <param name="ProjectId">Идентификатор проекта.</param>
public record DeleteProjectCommand(Guid ProjectId);

/// <summary>
/// Обработчик команды удаления проекта.
/// </summary>
public class DeleteProjectCommandHandler
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectCommandHandler(
        IRepository<Project> projectRepository,
        IUnitOfWork unitOfWork)
    {
        _projectRepository = Guard.Against.Null(projectRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
    }

    /// <summary>
    /// Удаляет проект по идентификатору.
    /// </summary>
    /// <param name="request">Команда с идентификатором проекта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача удаления.</returns>
    /// <exception cref="ArgumentException">Если ProjectId является значением по умолчанию.</exception>
    /// <exception cref="InvalidOperationException">Если проект не найден.</exception>
    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.ProjectId);

        var project = await _projectRepository.GetByIdAsync(request.ProjectId, trackChanges: true, cancellationToken);
        ThrowIf.EntityIsNull(project, request.ProjectId);

        await _projectRepository.RemoveAsync(project, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}