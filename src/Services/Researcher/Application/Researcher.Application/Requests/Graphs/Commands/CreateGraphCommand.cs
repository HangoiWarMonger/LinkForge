using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;

namespace Researcher.Application.Requests.Graphs.Commands;

/// <summary>
/// Команда создания нового графа в проекте.
/// </summary>
/// <param name="ProjectId">Идентификатор проекта.</param>
/// <param name="Title">Название графа.</param>
/// <param name="Description">Описание графа (опционально).</param>
public record CreateGraphCommand(
    Guid ProjectId,
    string Title,
    string? Description
);

/// <summary>
/// Обработчик команды создания графа.
/// </summary>
public class CreateGraphCommandHandler
{
    private readonly IRepository<Graph> _graphRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateGraphCommandHandler(
        IRepository<Graph> graphRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _graphRepository = Guard.Against.Null(graphRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Создаёт новый граф и возвращает DTO.
    /// </summary>
    /// <param name="request">Команда с параметрами создания графа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO созданного графа.</returns>
    /// <exception cref="ArgumentException">Если ProjectId — значение по умолчанию, Title пустое или null.</exception>
    public async Task<GraphDto> Handle(
        CreateGraphCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.ProjectId);
        Guard.Against.NullOrWhiteSpace(request.Title);

        var graph = new Graph(
            Guid.NewGuid(),
            request.Title,
            request.Description,
            request.ProjectId);

        await _graphRepository.AddAsync(graph, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GraphDto>(graph);
    }
}