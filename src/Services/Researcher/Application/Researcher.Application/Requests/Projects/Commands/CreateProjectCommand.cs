using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;

namespace Researcher.Application.Requests.Projects.Commands;

/// <summary>
/// Команда на создание нового проекта.
/// </summary>
/// <param name="Name">Название проекта.</param>
/// <param name="Description">Описание проекта.</param>
public sealed record CreateProjectCommand(string Name, string? Description);

/// <summary>
/// Обработчик команды создания проекта.
/// </summary>
public class CreateProjectCommandHandler
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProjectCommandHandler(
        IRepository<Project> projectRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Создаёт новый проект, сохраняет его и возвращает DTO.
    /// </summary>
    /// <param name="request">Команда с параметрами создания.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO созданного проекта.</returns>
    /// <exception cref="ArgumentNullException">Если имя проекта пустое или null.</exception>
    public async Task<ProjectDto> Handle(
        CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.NullOrWhiteSpace(request.Name);

        var project = new Project(
            Guid.NewGuid(),
            request.Name,
            request.Description
        );

        await _projectRepository.AddAsync(project, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProjectDto>(project);
    }
}