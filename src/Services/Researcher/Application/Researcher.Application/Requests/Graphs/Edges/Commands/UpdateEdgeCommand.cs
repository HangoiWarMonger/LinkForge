using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Graphs.Edges.Commands;

/// <summary>
/// Команда полного обновления ребра.
/// </summary>
/// <param name="EdgeId">Идентификатор ребра.</param>
/// <param name="NewFromNodeId">Идентификатор нового узла-источника.</param>
/// <param name="NewToNodeId">Идентификатор нового узла-приёмника.</param>
/// <param name="NewType">Новый тип ребра.</param>
/// <param name="NewDescription">Новое описание ребра (опционально).</param>
public record UpdateEdgeCommand(
    Guid EdgeId,
    Guid NewFromNodeId,
    Guid NewToNodeId,
    string NewType,
    string? NewDescription
);

/// <summary>
/// Обработчик команды полного обновления ребра.
/// </summary>
public class UpdateEdgeCommandHandler
{
    private readonly IRepository<Edge> _edgeRepository;
    private readonly IRepository<Node> _nodeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateEdgeCommandHandler(
        IRepository<Edge> edgeRepository,
        IRepository<Node> nodeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _edgeRepository = Guard.Against.Null(edgeRepository);
        _nodeRepository = Guard.Against.Null(nodeRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Обновляет ребро и возвращает актуальный DTO.
    /// </summary>
    /// <param name="request">Команда с новыми данными ребра.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO обновлённого ребра.</returns>
    /// <exception cref="ArgumentException">Если EdgeId — значение по умолчанию или NewType пустой.</exception>
    /// <exception cref="InvalidOperationException">Если ребро или узлы не найдены.</exception>
    public async Task<EdgeDto> Handle(
        UpdateEdgeCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.EdgeId);
        Guard.Against.NullOrWhiteSpace(request.NewType);

        var edge = await _edgeRepository.GetByIdAsync(
            request.EdgeId,
            trackChanges: true,
            cancellationToken);
        ThrowIf.EntityIsNull(edge, request.EdgeId);

        var newFrom = await _nodeRepository.GetByIdAsync(
            request.NewFromNodeId,
            trackChanges: false,
            cancellationToken);
        ThrowIf.EntityIsNull(newFrom, request.NewFromNodeId);

        var newTo = await _nodeRepository.GetByIdAsync(
            request.NewToNodeId,
            trackChanges: false,
            cancellationToken);
        ThrowIf.EntityIsNull(newTo, request.NewToNodeId);

        edge.Update(
            request.NewType,
            request.NewDescription,
            newFrom,
            newTo);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<EdgeDto>(edge);
    }
}