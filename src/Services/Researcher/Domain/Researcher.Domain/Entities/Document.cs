using Ardalis.GuardClauses;
using FluentValidation;
using Researcher.Domain.Validation;

namespace Researcher.Domain.Entities;

/// <summary>
/// Представляет документ проекта с содержимым в формате Markdown и признаком внутреннего или внешнего документа.
/// </summary>
public sealed class Document : BaseEntity
{
    private static readonly IValidator<Document> Validator = new DocumentValidator();

    /// <summary>
    /// Заголовок документа.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Содержимое документа в формате Markdown.
    /// </summary>
    public string BodyMarkdown { get; private set; }

    /// <summary>
    /// Признак внутреннего документа (true - внутренний, false - внешний).
    /// </summary>
    public bool IsInternal { get; private set; }

    /// <summary>
    /// Дата и время последнего обновления документа.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; private set; }

    /// <summary>
    /// Идентификатор проекта, которому принадлежит документ.
    /// </summary>
    public Guid ProjectId { get; private set; }

    /// <summary>
    /// Проект, к которому принадлежит документ.
    /// </summary>
    public Project Project { get; private set; }

    /// <summary>
    /// Создаёт новый документ с указанными параметрами.
    /// </summary>
    /// <param name="id">Уникальный идентификатор документа.</param>
    /// <param name="title">Заголовок документа.</param>
    /// <param name="bodyMarkdown">Содержимое документа в формате Markdown.</param>
    /// <param name="isInternal">Признак внутреннего документа.</param>
    /// <param name="projectId">Идентификатор проекта.</param>
    public Document(
        Guid id,
        string title,
        string bodyMarkdown,
        bool isInternal,
        Guid projectId
    ) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(title);
        Guard.Against.NullOrWhiteSpace(bodyMarkdown);
        Guard.Against.Default(projectId);

        Title = title;
        BodyMarkdown = bodyMarkdown;
        IsInternal = isInternal;
        ProjectId = projectId;
        UpdatedAt = DateTimeOffset.UtcNow;

        Validator.ValidateAndThrow(this);
    }

    /// <summary>
    /// Конструктор для ORM или сериализации.
    /// </summary>
    private Document()
    {
    }

    /// <summary>
    /// Обновляет содержимое документа, флаг внутреннего документа и дату обновления с валидацией.
    /// </summary>
    /// <param name="newTitle">Новый заголовок документа.</param>
    /// <param name="newBodyMarkdown">Новое содержимое документа в формате Markdown.</param>
    /// <param name="newIsInternal">Новый флаг внутреннего документа.</param>
    public void Update(string newTitle, string newBodyMarkdown, bool newIsInternal)
    {
        Guard.Against.NullOrWhiteSpace(newTitle);
        Guard.Against.NullOrWhiteSpace(newBodyMarkdown);

        Title = newTitle;
        BodyMarkdown = newBodyMarkdown;
        IsInternal = newIsInternal;
        UpdatedAt = DateTimeOffset.UtcNow;

        Validator.ValidateAndThrow(this);
    }
}