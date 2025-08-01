using Ardalis.GuardClauses;
using FluentValidation;
using Researcher.Domain.Validation;

namespace Researcher.Domain.Entities;

public sealed class Document : BaseEntity
{
    private static readonly IValidator<Document> Validator = new DocumentValidator();

    public string Title { get; private set; }
    public string BodyMarkdown { get; private set; }
    public bool IsInternal { get; private set; }
    public DateTime CreatedAt => CreatedAtUtc;
    public DateTime UpdatedAt { get; private set; }

    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; }

    public Document(
        Guid id,
        string title,
        string bodyMarkdown,
        bool isInternal,
        Guid projectId
    ) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(title, nameof(title));
        Guard.Against.NullOrWhiteSpace(bodyMarkdown, nameof(bodyMarkdown));
        Guard.Against.Default(projectId, nameof(projectId));

        Title = title;
        BodyMarkdown = bodyMarkdown;
        IsInternal = isInternal;
        ProjectId = projectId;
        UpdatedAt = DateTime.UtcNow;

        Validator.ValidateAndThrow(this);
    }

    /// <summary>
    /// Полное обновление содержимого документа, обновляет текст и флаг IsInternal,
    /// а также дату обновления. Выполняет валидацию.
    /// </summary>
    public void Update(string newTitle, string newBodyMarkdown, bool newIsInternal)
    {
        Guard.Against.NullOrWhiteSpace(newTitle, nameof(newTitle));
        Guard.Against.NullOrWhiteSpace(newBodyMarkdown, nameof(newBodyMarkdown));

        Title = newTitle;
        BodyMarkdown = newBodyMarkdown;
        IsInternal = newIsInternal;
        UpdatedAt = DateTime.UtcNow;

        Validator.ValidateAndThrow(this);
    }
}