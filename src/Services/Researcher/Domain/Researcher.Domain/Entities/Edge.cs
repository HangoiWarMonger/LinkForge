using Ardalis.GuardClauses;
using FluentValidation;
using Researcher.Domain.Validation;

namespace Researcher.Domain.Entities;

public sealed class Edge : BaseEntity
{
    private static readonly IValidator<Edge> Validator = new EdgeValidator();
    public string Type { get; private set; }
    public string? Description { get; private set; }

    public Guid FromNodeId { get; private set; }
    public Node FromNode { get; private set; }

    public Guid ToNodeId { get; private set; }
    public Node ToNode { get; private set; }

    public Edge(
        Guid id,
        string type,
        string? description,
        Node fromNode,
        Node toNode
    ) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(type, nameof(type));
        Guard.Against.Null(fromNode, nameof(fromNode));
        Guard.Against.Null(toNode, nameof(toNode));

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

    /// <summary>
    /// Полное обновление связи: тип, описание, и узлы-источник/приёмник.
    /// </summary>
    public void Update(
        string newType,
        string? newDescription,
        Node newFromNode,
        Node newToNode
    )
    {
        Guard.Against.NullOrWhiteSpace(newType, nameof(newType));
        Guard.Against.Null(newFromNode, nameof(newFromNode));
        Guard.Against.Null(newToNode, nameof(newToNode));

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
}