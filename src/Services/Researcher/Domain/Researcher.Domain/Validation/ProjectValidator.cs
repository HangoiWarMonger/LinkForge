using FluentValidation;
using Researcher.Domain.Entities;

namespace Researcher.Domain.Validation;

/// <summary>
/// Валидатор для сущности Project.
/// Проверяет корректность названия и описания проекта.
/// </summary>
public class ProjectValidator : AbstractValidator<Project>
{
    /// <summary>
    /// Создаёт экземпляр валидатора с правилами валидации для Project.
    /// </summary>
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