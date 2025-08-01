using Ardalis.GuardClauses;
using FluentValidation;
using Researcher.Domain.Validation;

namespace Researcher.Domain.Entities;

/// <summary>
/// Представляет проект исследования с именем, описанием и коллекциями связанных сущностей.
/// </summary>
public sealed class Project : BaseEntity
{
    private static readonly IValidator<Project> Validator = new ProjectValidator();

    /// <summary>
    /// Название проекта.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Описание проекта.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Коллекция графов, принадлежащих проекту.
    /// </summary>
    public ICollection<Graph> Graphs { get; private set; } = new List<Graph>();

    /// <summary>
    /// Коллекция документов проекта.
    /// </summary>
    public ICollection<Document> Documents { get; private set; } = new List<Document>();

    /// <summary>
    /// Коллекция задач проекта.
    /// </summary>
    public ICollection<TaskItem> TaskItems { get; private set; } = new List<TaskItem>();

    /// <summary>
    /// Создаёт новый проект с указанным идентификатором, именем и описанием.
    /// </summary>
    /// <param name="id">Уникальный идентификатор проекта.</param>
    /// <param name="name">Название проекта.</param>
    /// <param name="description">Описание проекта.</param>
    public Project(Guid id, string name, string? description) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(name);

        Name = name;
        Description = description;

        Validator.ValidateAndThrow(this);
    }

    /// <summary>
    /// Обновляет название и описание проекта с валидацией.
    /// </summary>
    /// <param name="newName">Новое название проекта.</param>
    /// <param name="newDescription">Новое описание проекта.</param>
    public void Update(string newName, string? newDescription)
    {
        Guard.Against.NullOrWhiteSpace(newName);

        Name = newName;
        Description = newDescription;

        Validator.ValidateAndThrow(this);
    }
}