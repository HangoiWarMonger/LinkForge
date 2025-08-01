using FluentValidation;
using Researcher.Domain.Entities;

namespace Researcher.Domain.Validation;

/// <summary>
/// Валидатор для сущности Node.
/// Проверяет корректность заголовка, типа, позиции и идентификатора графа.
/// </summary>
public class NodeValidator : AbstractValidator<Node>
{
    /// <summary>
    /// Создаёт экземпляр валидатора с правилами валидации для Node.
    /// </summary>
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
            .WithMessage(ValidationMessages.PositionRequired);

        RuleFor(x => x.GraphId)
            .NotEmpty()
            .WithMessage(ValidationMessages.GraphIdRequired);
    }
}