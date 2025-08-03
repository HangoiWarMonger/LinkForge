using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Projects.Queries;

/// <summary>
/// Запрос деталей проекта по идентификатору.
/// </summary>
/// <param name="ProjectId">Идентификатор проекта.</param>
public record GetProjectByIdQuery(Guid ProjectId);

/// <summary>
/// Обработчик запроса получения проекта по Id.
/// </summary>
public class GetProjectByIdQueryHandler
{
    private readonly IRepository<Project> _projectRepository;
    private readonly IMapper _mapper;

    public GetProjectByIdQueryHandler(
        IRepository<Project> projectRepository,
        IMapper mapper)
    {
        _projectRepository = Guard.Against.Null(projectRepository);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Получает DTO проекта по идентификатору.
    /// </summary>
    /// <param name="request">Запрос с Id проекта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO проекта.</returns>
    /// <exception cref="ArgumentException">Если ProjectId — значение по умолчанию.</exception>
    /// <exception cref="InvalidOperationException">Если проект не найден.</exception>
    public async Task<ProjectDto> Handle(
        GetProjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Default(request.ProjectId);
        Guard.Against.Null(request);

        var project = await _projectRepository.GetByIdAsync(
            request.ProjectId,
            trackChanges: false,
            cancellationToken);

        ThrowIf.EntityIsNull(project, request.ProjectId);

        return _mapper.Map<ProjectDto>(project);
    }
}