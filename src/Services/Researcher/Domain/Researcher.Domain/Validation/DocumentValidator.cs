using FluentValidation;
using Researcher.Domain.Entities;

namespace Researcher.Domain.Validation;

public class DocumentValidator : AbstractValidator<Document>
{
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