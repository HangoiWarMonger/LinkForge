using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;
using Researcher.Domain.ValueObjects;

namespace Researcher.Application.Requests.Graphs.Nodes.Commands;

/// <summary>
/// Команда перемещения узла (только позиция).
/// </summary>
/// <param name="NodeId">Идентификатор узла.</param>
/// <param name="NewPosition">Новая позиция узла.</param>
public record MoveNodeCommand(
    Guid NodeId,
    PositionDto NewPosition
);

/// <summary>
/// Обработчик команды перемещения узла.
/// </summary>
public class MoveNodeCommandHandler
{
    private readonly IRepository<Node> _nodeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MoveNodeCommandHandler(
        IRepository<Node> nodeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _nodeRepository = Guard.Against.Null(nodeRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Обновляет позицию узла и возвращает актуальный DTO.
    /// </summary>
    /// <param name="request">Команда с новым положением узла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO обновлённого узла.</returns>
    /// <exception cref="ArgumentException">Если NodeId — значение по умолчанию.</exception>
    /// <exception cref="ArgumentNullException">Если NewPosition равен null.</exception>
    /// <exception cref="InvalidOperationException">Если узел не найден.</exception>
    public async Task<NodeDto> Handle(
        MoveNodeCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.NodeId);
        Guard.Against.Null(request.NewPosition);

        var node = await _nodeRepository.GetByIdAsync(
            request.NodeId,
            trackChanges: true,
            cancellationToken);

        ThrowIf.EntityIsNull(node, request.NodeId);

        var newPosition = new Position(request.NewPosition.X, request.NewPosition.Y);

        node.Update(
            node.Title,
            node.Description,
            node.Type,
            newPosition,
            node.GraphId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<NodeDto>(node);
    }
}