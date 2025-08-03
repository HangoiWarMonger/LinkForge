using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;
using Researcher.Domain.ValueObjects;

namespace Researcher.Application.Requests.Graphs.Nodes.Commands;

/// <summary>
/// Команда полного обновления узла (метаданные и позиция).
/// </summary>
/// <param name="NodeId">Идентификатор узла.</param>
/// <param name="NewTitle">Новое название узла.</param>
/// <param name="NewType">Новый тип узла.</param>
/// <param name="NewPosition">Новая позиция узла.</param>
/// <param name="NewDescription">Новое описание узла (опционально).</param>
public record UpdateNodeCommand(
    Guid NodeId,
    string NewTitle,
    string NewType,
    PositionDto NewPosition,
    string? NewDescription
);

/// <summary>
/// Обработчик команды полного обновления узла.
/// </summary>
public class UpdateNodeCommandHandler
{
    private readonly IRepository<Node> _nodeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateNodeCommandHandler(
        IRepository<Node> nodeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _nodeRepository = Guard.Against.Null(nodeRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Обновляет узел и возвращает актуальный DTO.
    /// </summary>
    /// <param name="request">Команда с новыми данными узла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO обновлённого узла.</returns>
    /// <exception cref="ArgumentException">Если NodeId — значение по умолчанию, NewTitle или NewType пусты.</exception>
    /// <exception cref="ArgumentNullException">Если NewPosition равен null.</exception>
    /// <exception cref="InvalidOperationException">Если узел не найден.</exception>
    public async Task<NodeDto> Handle(
        UpdateNodeCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.NodeId);
        Guard.Against.NullOrWhiteSpace(request.NewTitle);
        Guard.Against.NullOrWhiteSpace(request.NewType);
        Guard.Against.Null(request.NewPosition);

        var node = await _nodeRepository.GetByIdAsync(
            request.NodeId,
            trackChanges: true,
            cancellationToken);

        ThrowIf.EntityIsNull(node, request.NodeId);

        var newPosition = new Position(request.NewPosition.X, request.NewPosition.Y);

        node.Update(
            request.NewTitle,
            request.NewDescription,
            request.NewType,
            newPosition,
            node.GraphId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<NodeDto>(node);
    }
}