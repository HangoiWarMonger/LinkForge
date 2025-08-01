using Ardalis.GuardClauses;
using FluentValidation;
using Researcher.Domain.Validation;
using Researcher.Domain.ValueObjects;

namespace Researcher.Domain.Entities;

/// <summary>
/// Представляет задачу проекта с иерархией вложенности и статусом выполнения.
/// </summary>
public sealed class TaskItem : BaseEntity
{
    private static readonly IValidator<TaskItem> Validator = new TaskItemValidator();

    /// <summary>
    /// Заголовок задачи.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Описание задачи.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Статус задачи.
    /// </summary>
    public TaskItemStatus Status { get; private set; }

    /// <summary>
    /// Идентификатор проекта, к которому принадлежит задача.
    /// </summary>
    public Guid ProjectId { get; private set; }

    /// <summary>
    /// Идентификатор родительской задачи (если есть).
    /// </summary>
    public Guid? ParentId { get; private set; }

    /// <summary>
    /// Родительская задача (если есть).
    /// </summary>
    public TaskItem? Parent { get; private set; }

    /// <summary>
    /// Коллекция дочерних задач.
    /// </summary>
    public ICollection<TaskItem> Children { get; private set; } = new List<TaskItem>();

    /// <summary>
    /// Создаёт новую задачу с указанными параметрами.
    /// </summary>
    /// <param name="id">Уникальный идентификатор задачи.</param>
    /// <param name="title">Заголовок задачи.</param>
    /// <param name="description">Описание задачи.</param>
    /// <param name="status">Статус задачи.</param>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <param name="parent">Родительская задача (опционально).</param>
    public TaskItem(
        Guid id,
        string title,
        string? description,
        TaskItemStatus status,
        Guid projectId,
        TaskItem? parent = null
    ) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(title);
        Guard.Against.Default(projectId);
        Guard.Against.EnumOutOfRange(status);

        Title = title;
        Description = description;
        Status = status;
        ProjectId = projectId;

        if (parent is not null)
        {
            Parent = parent;
            ParentId = parent.Id;
            parent.Children.Add(this);
        }

        Validator.ValidateAndThrow(this);
    }

    /// <summary>
    /// Возвращает глубину вложенности задачи в иерархии.
    /// </summary>
    /// <returns>Глубина вложенности задачи.</returns>
    public int GetDepth()
    {
        return Parent is null ? 1 : Parent.GetDepth() + 1;
    }

    /// <summary>
    /// Полное обновление задачи с проверками и валидацией.
    /// </summary>
    /// <param name="newTitle">Новый заголовок задачи.</param>
    /// <param name="newDescription">Новое описание задачи.</param>
    /// <param name="newStatus">Новый статус задачи.</param>
    /// <param name="newParent">Новая родительская задача (опционально).</param>
    public void Update(
        string newTitle,
        string? newDescription,
        TaskItemStatus newStatus,
        TaskItem? newParent = null
    )
    {
        Guard.Against.NullOrWhiteSpace(newTitle);
        Guard.Against.EnumOutOfRange(newStatus);

        // Удаляем из старого родителя, если есть
        if (Parent != null)
        {
            Parent.Children.Remove(this);
        }

        Parent = newParent;
        ParentId = newParent?.Id;

        if (newParent != null)
        {
            newParent.Children.Add(this);
        }

        Title = newTitle;
        Description = newDescription;
        Status = newStatus;

        Validator.ValidateAndThrow(this);
    }
}