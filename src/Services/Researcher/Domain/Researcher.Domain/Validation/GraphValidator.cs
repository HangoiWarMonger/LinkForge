using FluentValidation;
using Researcher.Domain.Entities;

namespace Researcher.Domain.Validation;

public class GraphValidator : AbstractValidator<Graph>
{
    public GraphValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(ValidationMessages.TitleRequired)
            .MaximumLength(ValidationConstants.TitleMaxLength)
            .WithMessage(ValidationMessages.TitleMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(ValidationConstants.DescriptionMaxLength)
            .WithMessage(ValidationMessages.DescriptionMaxLength);

        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage(ValidationMessages.ProjectIdRequired);
    }
}