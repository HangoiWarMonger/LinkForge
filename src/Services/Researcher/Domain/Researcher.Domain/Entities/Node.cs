using Ardalis.GuardClauses;
using FluentValidation;
using Researcher.Domain.Validation;
using Researcher.Domain.ValueObjects;

namespace Researcher.Domain.Entities;

public sealed class Node : BaseEntity
{
    private static readonly IValidator<Node> _validator = new NodeValidator();

    public string Title { get; private set; }
    public string? Description { get; private set; }
    public string Type { get; private set; }
    public Position Position { get; private set; }
    public Guid GraphId { get; private set; }

    public ICollection<Edge> OutgoingEdges { get; private set; } = new List<Edge>();
    public ICollection<Edge> IncomingEdges { get; private set; } = new List<Edge>();

    public Node(
        Guid id,
        string title,
        string? description,
        string type,
        Position position,
        Guid graphId
    ) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(title, nameof(title));
        Guard.Against.NullOrWhiteSpace(type, nameof(type));
        Guard.Against.Null(position, nameof(position));
        Guard.Against.Default(graphId, nameof(graphId));

        Title = title;
        Description = description;
        Type = type;
        Position = position;
        GraphId = graphId;

        _validator.ValidateAndThrow(this);
    }
    
    /// <summary>
    /// Полное обновление узла с валидацией.
    /// </summary>
    public void Update(
        string newTitle,
        string? newDescription,
        string newType,
        Position newPosition,
        Guid newGraphId
    )
    {
        Guard.Against.NullOrWhiteSpace(newTitle, nameof(newTitle));
        Guard.Against.NullOrWhiteSpace(newType, nameof(newType));
        Guard.Against.Null(newPosition, nameof(newPosition));
        Guard.Against.Default(newGraphId, nameof(newGraphId));

        Title = newTitle;
        Description = newDescription;
        Type = newType;
        Position = newPosition;
        GraphId = newGraphId;

        _validator.ValidateAndThrow(this);
    }
}