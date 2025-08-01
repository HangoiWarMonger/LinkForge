using FluentValidation;
using Researcher.Domain.Entities;

namespace Researcher.Domain.Validation;

public class EdgeValidator : AbstractValidator<Edge>
{
    public EdgeValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage(ValidationMessages.TypeRequired)
            .MaximumLength(ValidationConstants.TypeMaxLength)
            .WithMessage(ValidationMessages.TypeMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(ValidationConstants.DescriptionMaxLength)
            .WithMessage(ValidationMessages.DescriptionMaxLength);

        RuleFor(x => x.FromNode)
            .NotNull()
            .WithMessage(ValidationMessages.FromNodeRequired);

        RuleFor(x => x.ToNode)
            .NotNull()
            .WithMessage(ValidationMessages.ToNodeRequired);

        RuleFor(x => x)
            .Must(edge =>
                edge.FromNode.OutgoingEdges.Count(e => e.ToNodeId == edge.ToNodeId)
                + edge.ToNode.OutgoingEdges.Count(e => e.ToNodeId == edge.FromNodeId)
                <= ValidationConstants.MaxEdgesBetweenNodes
            )
            .WithMessage(edge => ValidationMessages.MaxEdgesBetweenNodesExceeded(edge.FromNodeId, edge.ToNodeId));
    }
}