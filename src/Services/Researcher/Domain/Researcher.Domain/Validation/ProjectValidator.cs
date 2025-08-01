using FluentValidation;
using Researcher.Domain.Entities;

namespace Researcher.Domain.Validation;

public class ProjectValidator : AbstractValidator<Project>
{
    public ProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ValidationMessages.NameRequired)
            .MaximumLength(ValidationConstants.NameMaxLength)
            .WithMessage(ValidationMessages.NameMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(ValidationConstants.DescriptionMaxLength)
            .WithMessage(ValidationMessages.DescriptionMaxLength);
    }
}