using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.ValueObjects;

namespace Researcher.Application.Requests.Graphs.Nodes.Commands;

/// <summary>
/// Команда создания нового узла в графе.
/// </summary>
/// <param name="GraphId">Идентификатор графа.</param>
/// <param name="Title">Название узла.</param>
/// <param name="Type">Тип узла.</param>
/// <param name="Position">Позиция узла.</param>
/// <param name="Description">Описание узла (опционально).</param>
public record CreateNodeCommand(
    Guid GraphId,
    string Title,
    string Type,
    PositionDto Position,
    string? Description
);

/// <summary>
/// Обработчик команды создания узла.
/// </summary>
public class CreateNodeCommandHandler
{
    private readonly IRepository<Node> _nodeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateNodeCommandHandler(
        IRepository<Node> nodeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _nodeRepository = Guard.Against.Null(nodeRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Создаёт новый узел и возвращает DTO.
    /// </summary>
    /// <param name="request">Команда с параметрами узла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO созданного узла.</returns>
    /// <exception cref="ArgumentException">Если GraphId — значение по умолчанию, Title или Type пусты.</exception>
    /// <exception cref="ArgumentNullException">Если Position равен null.</exception>
    public async Task<NodeDto> Handle(
        CreateNodeCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.GraphId);
        Guard.Against.NullOrWhiteSpace(request.Title);
        Guard.Against.NullOrWhiteSpace(request.Type);
        Guard.Against.Null(request.Position);

        var position = new Position(request.Position.X, request.Position.Y);

        var node = new Node(
            Guid.NewGuid(),
            request.Title,
            request.Description,
            request.Type,
            position,
            request.GraphId);

        await _nodeRepository.AddAsync(node, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<NodeDto>(node);
    }
}