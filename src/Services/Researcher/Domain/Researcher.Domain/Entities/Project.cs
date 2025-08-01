using Ardalis.GuardClauses;
using FluentValidation;
using Researcher.Domain.Validation;

namespace Researcher.Domain.Entities;

public sealed class Project : BaseEntity
{
    private static readonly IValidator<Project> Validator = new ProjectValidator();

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt => CreatedAtUtc;

    public ICollection<Graph> Graphs { get; private set; } = new List<Graph>();
    public ICollection<Document> Documents { get; private set; } = new List<Document>();
    public ICollection<TaskItem> TaskItems { get; private set; } = new List<TaskItem>();

    public Project(Guid id, string name, string? description) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));

        Name = name;
        Description = description;

        Validator.ValidateAndThrow(this);
    }

    /// <summary>
    /// Полное обновление проекта с валидацией.
    /// </summary>
    public void Update(string newName, string? newDescription)
    {
        Guard.Against.NullOrWhiteSpace(newName, nameof(newName));

        Name = newName;
        Description = newDescription;

        Validator.ValidateAndThrow(this);
    }
}