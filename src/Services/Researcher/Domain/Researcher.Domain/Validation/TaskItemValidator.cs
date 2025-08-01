using FluentValidation;
using Researcher.Domain.Entities;
using Researcher.Domain.ValueObjects;

namespace Researcher.Domain.Validation;

public class TaskItemValidator : AbstractValidator<TaskItem>
{
    public TaskItemValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(ValidationMessages.TitleRequired)
            .MaximumLength(ValidationConstants.TitleMaxLength)
            .WithMessage(ValidationMessages.TitleMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(ValidationConstants.DescriptionMaxLength)
            .WithMessage(ValidationMessages.DescriptionMaxLength);

        RuleFor(x => x.Status)
            .IsInEnum()
            .NotEqual(TaskItemStatus.Undefined)
            .WithMessage(ValidationMessages.InvalidStatus);

        RuleFor(x => x)
            .Must(task => task.GetDepth() <= ValidationConstants.MaxDepth)
            .WithMessage(ValidationMessages.DepthExceeded);
    }
}