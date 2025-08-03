using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Projects.Commands;

/// <summary>
/// Команда обновления проекта.
/// </summary>
/// <param name="ProjectId">Идентификатор проекта.</param>
/// <param name="NewName">Новое название проекта.</param>
/// <param name="NewDescription">Новое описание проекта.</param>
public record UpdateProjectCommand(Guid ProjectId, string NewName, string? NewDescription);

/// <summary>
/// Обработчик команды обновления проекта.
/// </summary>
public class UpdateProjectCommandHandler
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProjectCommandHandler(
        IRepository<Project> projectRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _projectRepository = Guard.Against.Null(projectRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Обновляет проект и возвращает DTO с актуальными данными.
    /// </summary>
    /// <param name="request">Команда с параметрами обновления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO обновленного проекта.</returns>
    /// <exception cref="ArgumentException">Если ProjectId — значение по умолчанию или NewName пустой.</exception>
    /// <exception cref="InvalidOperationException">Если проект не найден.</exception>
    public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.ProjectId);
        Guard.Against.NullOrWhiteSpace(request.NewName);

        var project = await _projectRepository.GetByIdAsync(
            request.ProjectId,
            trackChanges: true,
            cancellationToken);

        ThrowIf.EntityIsNull(project, request.ProjectId);

        project.Update(request.NewName, request.NewDescription);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProjectDto>(project);
    }
}