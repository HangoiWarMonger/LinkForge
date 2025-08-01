using FluentValidation;
using Researcher.Domain.Entities;

namespace Researcher.Domain.Validation;

/// <summary>
/// Валидатор для сущности Document.
/// Проверяет корректность заголовка, содержимого и идентификатора проекта.
/// </summary>
public class DocumentValidator : AbstractValidator<Document>
{
    /// <summary>
    /// Создаёт экземпляр валидатора с правилами валидации для Document.
    /// </summary>
    public DocumentValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(ValidationMessages.TitleRequired)
            .MaximumLength(ValidationConstants.TitleMaxLength)
            .WithMessage(ValidationMessages.TitleMaxLength);

        RuleFor(x => x.BodyMarkdown)
            .NotEmpty()
            .WithMessage(ValidationMessages.BodyMarkdownRequired)
            .MaximumLength(ValidationConstants.BodyMarkdownMaxLength)
            .WithMessage(ValidationMessages.BodyMarkdownMaxLength);

        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage(ValidationMessages.ProjectIdRequired);
    }
}