using Ardalis.GuardClauses;
using FluentValidation;
using Researcher.Domain.Validation;

namespace Researcher.Domain.Entities;

public sealed class Graph : BaseEntity
{
    private static readonly IValidator<Graph> _validator = new GraphValidator();

    public string Title { get; private set; }
    public string? Description { get; private set; }
    public Guid ProjectId { get; private set; }

    public ICollection<Node> Nodes { get; private set; } = new List<Node>();
    public ICollection<Edge> Edges { get; private set; } = new List<Edge>();

    public Graph(
        Guid id,
        string title,
        string? description,
        Guid projectId
    ) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(title, nameof(title));
        Guard.Against.Default(projectId, nameof(projectId));

        Title = title;
        Description = description;
        ProjectId = projectId;

        _validator.ValidateAndThrow(this);
    }

    /// <summary>
    /// Полное обновление графа с валидацией.
    /// </summary>
    public void Update(
        string newTitle,
        string? newDescription,
        Guid newProjectId
    )
    {
        Guard.Against.NullOrWhiteSpace(newTitle, nameof(newTitle));
        Guard.Against.Default(newProjectId, nameof(newProjectId));

        Title = newTitle;
        Description = newDescription;
        ProjectId = newProjectId;

        _validator.ValidateAndThrow(this);
    }
}