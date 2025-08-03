using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Graphs.Commands;

/// <summary>
/// Команда обновления заголовка и описания графа.
/// </summary>
/// <param name="GraphId">Идентификатор графа.</param>
/// <param name="NewTitle">Новое название графа.</param>
/// <param name="NewDescription">Новое описание графа (опционально).</param>
public record UpdateGraphCommand(
    Guid GraphId,
    string NewTitle,
    string? NewDescription
);

/// <summary>
/// Обработчик команды обновления графа.
/// </summary>
public class UpdateGraphCommandHandler
{
    private readonly IRepository<Graph> _graphRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateGraphCommandHandler(
        IRepository<Graph> graphRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _graphRepository = Guard.Against.Null(graphRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Обновляет заголовок и описание графа, возвращает DTO.
    /// </summary>
    /// <param name="request">Команда с новыми данными графа.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>DTO обновлённого графа.</returns>
    /// <exception cref="ArgumentException">Если GraphId — значение по умолчанию или NewTitle пустое.</exception>
    /// <exception cref="InvalidOperationException">Если граф не найден.</exception>
    public async Task<GraphDto> Handle(
        UpdateGraphCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.GraphId);
        Guard.Against.NullOrWhiteSpace(request.NewTitle);

        var graph = await _graphRepository.GetByIdAsync(
            request.GraphId,
            trackChanges: true,
            cancellationToken);

        ThrowIf.EntityIsNull(graph, request.GraphId);

        graph.Update(
            request.NewTitle,
            request.NewDescription,
            graph.ProjectId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GraphDto>(graph);
    }
}