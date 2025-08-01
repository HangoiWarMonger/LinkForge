using Ardalis.GuardClauses;
using FluentValidation;
using Researcher.Domain.Validation;

namespace Researcher.Domain.Entities;

/// <summary>
/// Представляет ребро (связь) между двумя узлами графа.
/// </summary>
public sealed class Edge : BaseEntity
{
    private static readonly IValidator<Edge> Validator = new EdgeValidator();

    /// <summary>
    /// Тип связи.
    /// </summary>
    public string Type { get; private set; }

    /// <summary>
    /// Описание связи.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Идентификатор узла-источника.
    /// </summary>
    public Guid FromNodeId { get; private set; }

    /// <summary>
    /// Узел-источник связи.
    /// </summary>
    public Node FromNode { get; private set; }

    /// <summary>
    /// Идентификатор узла-приёмника.
    /// </summary>
    public Guid ToNodeId { get; private set; }

    /// <summary>
    /// Узел-приёмник связи.
    /// </summary>
    public Node ToNode { get; private set; }

    /// <summary>
    /// Создаёт новый экземпляр связи между двумя узлами.
    /// </summary>
    /// <param name="id">Уникальный идентификатор связи.</param>
    /// <param name="type">Тип связи.</param>
    /// <param name="description">Описание связи.</param>
    /// <param name="fromNode">Узел-источник.</param>
    /// <param name="toNode">Узел-приёмник.</param>
    public Edge(
        Guid id,
        string type,
        string? description,
        Node fromNode,
        Node toNode
    ) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(type);
        Guard.Against.Null(fromNode);
        Guard.Against.Null(toNode);

        Type = type;
        Description = description;

        FromNode = fromNode;
        FromNodeId = fromNode.Id;

        ToNode = toNode;
        ToNodeId = toNode.Id;

        fromNode.OutgoingEdges.Add(this);
        toNode.IncomingEdges.Add(this);

        Validator.ValidateAndThrow(this);
    }

    private Edge()
    {
    }

    /// <summary>
    /// Полное обновление связи: тип, описание, и узлы-источник и приёмник.
    /// </summary>
    /// <param name="newType">Новый тип связи.</param>
    /// <param name="newDescription">Новое описание связи.</param>
    /// <param name="newFromNode">Новый узел-источник.</param>
    /// <param name="newToNode">Новый узел-приёмник.</param>
    public void Update(
        string newType,
        string? newDescription,
        Node newFromNode,
        Node newToNode
    )
    {
        Guard.Against.NullOrWhiteSpace(newType);
        Guard.Against.Null(newFromNode);
        Guard.Against.Null(newToNode);

        DetachFromNodes();

        Type = newType;
        Description = newDescription;
        FromNode = newFromNode;
        FromNodeId = newFromNode.Id;
        ToNode = newToNode;
        ToNodeId = newToNode.Id;

        AttachToNodes();

        Validator.ValidateAndThrow(this);
    }

    private void DetachFromNodes()
    {
        FromNode.OutgoingEdges.Remove(this);
        ToNode.IncomingEdges.Remove(this);
    }

    private void AttachToNodes()
    {
        FromNode.OutgoingEdges.Add(this);
        ToNode.IncomingEdges.Add(this);
    }
}