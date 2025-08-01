using FluentValidation;
using Researcher.Domain.Entities;

namespace Researcher.Domain.Validation;

/// <summary>
/// Валидатор для сущности Graph.
/// Проверяет корректность заголовка и идентификатора проекта.
/// </summary>
public class GraphValidator : AbstractValidator<Graph>
{
    /// <summary>
    /// Создаёт экземпляр валидатора с правилами валидации для Graph.
    /// </summary>
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