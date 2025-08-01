using Ardalis.GuardClauses;
using FluentValidation;
using Researcher.Domain.Validation;

namespace Researcher.Domain.Entities;

public sealed class TaskItem : BaseEntity
{
    private static readonly IValidator<TaskItem> Validator = new TaskItemValidator();

    public string Title { get; private set; }
    public string? Description { get; private set; }
    public TaskStatus Status { get; private set; }
    public Guid ProjectId { get; private set; }

    public Guid? ParentId { get; private set; }
    public TaskItem? Parent { get; private set; }
    public ICollection<TaskItem> Children { get; private set; } = new List<TaskItem>();

    public TaskItem(
        Guid id,
        string title,
        string? description,
        TaskStatus status,
        Guid projectId,
        TaskItem? parent = null
    ) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(title, nameof(title));
        Guard.Against.Default(projectId, nameof(projectId));
        Guard.Against.EnumOutOfRange(status, nameof(status));

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

    internal int GetDepth()
    {
        return Parent is null ? 1 : Parent.GetDepth() + 1;
    }
    
    /// <summary>
    /// Полное обновление задачи с проверками и валидацией.
    /// </summary>
    public void Update(
        string newTitle,
        string? newDescription,
        TaskStatus newStatus,
        TaskItem? newParent = null
    )
    {
        Guard.Against.NullOrWhiteSpace(newTitle, nameof(newTitle));
        Guard.Against.EnumOutOfRange(newStatus, nameof(newStatus));

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