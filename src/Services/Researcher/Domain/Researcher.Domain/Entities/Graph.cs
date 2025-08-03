using Ardalis.GuardClauses;
using FluentValidation;
using Researcher.Domain.Validation;

namespace Researcher.Domain.Entities;

/// <summary>
/// Представляет граф, содержащий узлы и ребра, принадлежащий проекту.
/// </summary>
public sealed class Graph : BaseEntity
{
    private static readonly IValidator<Graph> Validator = new GraphValidator();

    /// <summary>
    /// Заголовок графа.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Описание графа.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Идентификатор проекта, которому принадлежит граф.
    /// </summary>
    public Guid ProjectId { get; private set; }

    /// <summary>
    /// Коллекция узлов, входящих в граф.
    /// </summary>
    public ICollection<Node> Nodes { get; private set; } = new List<Node>();

    /// <summary>
    /// Коллекция ребер графа.
    /// </summary>
    public ICollection<Edge> Edges { get; private set; } = new List<Edge>();

    /// <summary>
    /// Создаёт новый граф с заданными параметрами.
    /// </summary>
    /// <param name="id">Уникальный идентификатор графа.</param>
    /// <param name="title">Заголовок графа.</param>
    /// <param name="description">Описание графа.</param>
    /// <param name="projectId">Идентификатор проекта.</param>
    public Graph(
        Guid id,
        string title,
        string? description,
        Guid projectId
    ) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(title);
        Guard.Against.Default(projectId);

        Title = title;
        Description = description;
        ProjectId = projectId;

        Validator.ValidateAndThrow(this);
    }

    /// <summary>
    /// Конструктор для ORM или сериализации.
    /// </summary>
    private Graph()
    {
    }

    /// <summary>
    /// Полное обновление графа с валидацией.
    /// </summary>
    /// <param name="newTitle">Новый заголовок графа.</param>
    /// <param name="newDescription">Новое описание графа.</param>
    /// <param name="newProjectId">Новый идентификатор проекта.</param>
    public void Update(
        string newTitle,
        string? newDescription,
        Guid newProjectId
    )
    {
        Guard.Against.NullOrWhiteSpace(newTitle);
        Guard.Against.Default(newProjectId);

        Title = newTitle;
        Description = newDescription;
        ProjectId = newProjectId;

        Validator.ValidateAndThrow(this);
    }
}