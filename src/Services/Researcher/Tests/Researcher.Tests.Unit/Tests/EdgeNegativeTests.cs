using FluentAssertions;
using FluentValidation;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;
using Researcher.Domain.ValueObjects;

namespace Researcher.Tests.Unit.Tests;

public class EdgeNegativeTests
{
    private static Node CreateNode(Guid? id = null)
    {
        return new Node(
            id ?? Guid.NewGuid(),
            "Node Title",
            "Some description",
            "NodeType",
            new Position(1.0, 2.0),
            Guid.NewGuid());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_Should_Throw_When_TypeIsNullOrWhitespace(string invalidType)
    {
        // Arrange
        var fromNode = CreateNode();
        var toNode = CreateNode();

        // Act & Assert
        FluentActions.Invoking(() => new Edge(Guid.NewGuid(), invalidType, null, fromNode, toNode))
            .Should().Throw<ArgumentException>()
            .WithMessage("*type*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_FromNodeIsNull()
    {
        var toNode = CreateNode();

        FluentActions.Invoking(() => new Edge(Guid.NewGuid(), "Type", null, null!, toNode))
            .Should().Throw<ArgumentNullException>()
            .WithMessage("*fromNode*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_ToNodeIsNull()
    {
        var fromNode = CreateNode();

        FluentActions.Invoking(() => new Edge(Guid.NewGuid(), "Type", null, fromNode, null!))
            .Should().Throw<ArgumentNullException>()
            .WithMessage("*toNode*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_Should_Throw_When_NewTypeIsNullOrWhitespace(string invalidType)
    {
        var fromNode = CreateNode();
        var toNode = CreateNode();

        var edge = new Edge(Guid.NewGuid(), "Type", null, fromNode, toNode);

        FluentActions.Invoking(() => edge.Update(invalidType, null, fromNode, toNode))
            .Should().Throw<ArgumentException>()
            .WithMessage("*newType*");
    }

    [Fact]
    public void Update_Should_Throw_When_NewFromNodeIsNull()
    {
        var fromNode = CreateNode();
        var toNode = CreateNode();

        var edge = new Edge(Guid.NewGuid(), "Type", null, fromNode, toNode);

        FluentActions.Invoking(() => edge.Update("NewType", null, null!, toNode))
            .Should().Throw<ArgumentNullException>()
            .WithMessage("*newFromNode*");
    }

    [Fact]
    public void Update_Should_Throw_When_NewToNodeIsNull()
    {
        var fromNode = CreateNode();
        var toNode = CreateNode();

        var edge = new Edge(Guid.NewGuid(), "Type", null, fromNode, toNode);

        FluentActions.Invoking(() => edge.Update("NewType", null, fromNode, null!))
            .Should().Throw<ArgumentNullException>()
            .WithMessage("*newToNode*");
    }

    [Fact]
    public void Constructor_Should_ThrowValidationException_When_TypeExceedsMaxLength()
    {
        var fromNode = CreateNode();
        var toNode = CreateNode();

        var longType = new string('T', ValidationConstants.TitleMaxLength + 1);

        FluentActions.Invoking(() => new Edge(Guid.NewGuid(), longType, null, fromNode, toNode))
            .Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Type"));
    }
}