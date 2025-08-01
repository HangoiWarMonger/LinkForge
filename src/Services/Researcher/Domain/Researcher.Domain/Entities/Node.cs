using Ardalis.GuardClauses;
using FluentValidation;
using Researcher.Domain.Validation;
using Researcher.Domain.ValueObjects;

namespace Researcher.Domain.Entities;

/// <summary>
/// Представляет узел графа с заголовком, описанием, типом и позицией.
/// </summary>
public sealed class Node : BaseEntity
{
    private static readonly IValidator<Node> Validator = new NodeValidator();

    /// <summary>
    /// Заголовок узла.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Описание узла.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Тип узла.
    /// </summary>
    public string Type { get; private set; }

    /// <summary>
    /// Позиция узла на плоскости.
    /// </summary>
    public Position Position { get; private set; }

    /// <summary>
    /// Идентификатор графа, которому принадлежит узел.
    /// </summary>
    public Guid GraphId { get; private set; }

    /// <summary>
    /// Коллекция исходящих ребер от данного узла.
    /// </summary>
    public ICollection<Edge> OutgoingEdges { get; private set; } = new List<Edge>();

    /// <summary>
    /// Коллекция входящих ребер к данному узлу.
    /// </summary>
    public ICollection<Edge> IncomingEdges { get; private set; } = new List<Edge>();

    /// <summary>
    /// Создаёт новый узел с заданными параметрами.
    /// </summary>
    /// <param name="id">Уникальный идентификатор узла.</param>
    /// <param name="title">Заголовок узла.</param>
    /// <param name="description">Описание узла.</param>
    /// <param name="type">Тип узла.</param>
    /// <param name="position">Позиция узла.</param>
    /// <param name="graphId">Идентификатор графа.</param>
    public Node(
        Guid id,
        string title,
        string? description,
        string type,
        Position position,
        Guid graphId
    ) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(title);
        Guard.Against.NullOrWhiteSpace(type);
        Guard.Against.Null(position);
        Guard.Against.Default(graphId);

        Title = title;
        Description = description;
        Type = type;
        Position = position;
        GraphId = graphId;

        Validator.ValidateAndThrow(this);
    }

    /// <summary>
    /// Полное обновление узла с валидацией.
    /// </summary>
    /// <param name="newTitle">Новый заголовок узла.</param>
    /// <param name="newDescription">Новое описание узла.</param>
    /// <param name="newType">Новый тип узла.</param>
    /// <param name="newPosition">Новая позиция узла.</param>
    /// <param name="newGraphId">Новый идентификатор графа.</param>
    public void Update(
        string newTitle,
        string? newDescription,
        string newType,
        Position newPosition,
        Guid newGraphId
    )
    {
        Guard.Against.NullOrWhiteSpace(newTitle);
        Guard.Against.NullOrWhiteSpace(newType);
        Guard.Against.Null(newPosition);
        Guard.Against.Default(newGraphId);

        Title = newTitle;
        Description = newDescription;
        Type = newType;
        Position = newPosition;
        GraphId = newGraphId;

        Validator.ValidateAndThrow(this);
    }
}