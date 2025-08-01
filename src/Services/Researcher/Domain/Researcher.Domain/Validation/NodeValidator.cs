using FluentValidation;
using Researcher.Domain.Entities;

namespace Researcher.Domain.Validation;

public class NodeValidator : AbstractValidator<Node>
{
    public NodeValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(ValidationMessages.TitleRequired)
            .MaximumLength(ValidationConstants.TitleMaxLength)
            .WithMessage(ValidationMessages.TitleMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(ValidationConstants.DescriptionMaxLength)
            .WithMessage(ValidationMessages.DescriptionMaxLength);

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage(ValidationMessages.TypeRequired)
            .MaximumLength(ValidationConstants.TypeMaxLength)
            .WithMessage(ValidationMessages.TypeMaxLength);

        RuleFor(x => x.Position)
            .NotNull()
            .WithMessage("Position must be provided.");

        RuleFor(x => x.GraphId)
            .NotEmpty()
            .WithMessage("GraphId is required.");
    }
}