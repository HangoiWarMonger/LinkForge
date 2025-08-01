using FluentAssertions;
using FluentValidation;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;
using Researcher.Domain.ValueObjects;

namespace Researcher.Tests.Unit.Tests;

/// <summary>
/// Набор негативных юнит-тестов для проверки конструктора и метода Update сущности Edge на корректную обработку ошибок.
/// </summary>
public class EdgeNegativeTests
{
    /// <summary>
    /// Проверяет, что конструктор выбрасывает ArgumentException при передаче null, пустой строки или строки из пробелов в параметр type.
    /// </summary>
    /// <param name="invalidType">Недопустимое значение типа.</param>
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

    /// <summary>
    /// Проверяет, что конструктор выбрасывает ArgumentNullException при передаче null в параметр fromNode.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_FromNodeIsNull()
    {
        var toNode = CreateNode();

        FluentActions.Invoking(() => new Edge(Guid.NewGuid(), "Type", null, null!, toNode))
            .Should().Throw<ArgumentNullException>()
            .WithMessage("*fromNode*");
    }

    /// <summary>
    /// Проверяет, что конструктор выбрасывает ArgumentNullException при передаче null в параметр toNode.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_ToNodeIsNull()
    {
        var fromNode = CreateNode();

        FluentActions.Invoking(() => new Edge(Guid.NewGuid(), "Type", null, fromNode, null!))
            .Should().Throw<ArgumentNullException>()
            .WithMessage("*toNode*");
    }

    /// <summary>
    /// Проверяет, что метод Update выбрасывает ArgumentException при передаче null, пустой строки или строки из пробелов в параметр newType.
    /// </summary>
    /// <param name="invalidType">Недопустимое значение нового типа.</param>
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

    /// <summary>
    /// Проверяет, что метод Update выбрасывает ArgumentNullException при передаче null в параметр newFromNode.
    /// </summary>
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

    /// <summary>
    /// Проверяет, что метод Update выбрасывает ArgumentNullException при передаче null в параметр newToNode.
    /// </summary>
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

    /// <summary>
    /// Проверяет, что конструктор выбрасывает ValidationException при превышении максимальной длины поля Type.
    /// </summary>
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

    /// <summary>
    /// Вспомогательный метод для создания валидного узла Node с опциональным заданием идентификатора.
    /// </summary>
    /// <param name="id">Опциональный идентификатор узла.</param>
    /// <returns>Созданный валидный узел Node.</returns>
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
}