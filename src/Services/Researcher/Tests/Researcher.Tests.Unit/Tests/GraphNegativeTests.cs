using FluentAssertions;
using FluentValidation;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Tests.Unit.Tests;

/// <summary>
/// Набор негативных юнит-тестов для проверки конструктора и метода Update сущности Graph на обработку некорректных данных.
/// </summary>
public class GraphNegativeTests
{
    /// <summary>
    /// Проверяет, что конструктор выбрасывает ArgumentException при передаче null, пустой строки или строки из пробелов в параметр title.
    /// </summary>
    /// <param name="invalidTitle">Недопустимое значение заголовка.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void Constructor_Should_Throw_When_TitleIsNullOrWhitespace(string invalidTitle)
    {
        // Arrange
        var id = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        // Act & Assert
        FluentActions.Invoking(() => new Graph(id, invalidTitle!, "desc", projectId))
            .Should().Throw<ArgumentException>()
            .WithMessage("*title*");
    }

    /// <summary>
    /// Проверяет, что конструктор выбрасывает ArgumentException при передаче значения по умолчанию в параметр projectId.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_ProjectIdIsDefault()
    {
        // Arrange
        var id = Guid.NewGuid();
        var defaultProjectId = default(Guid);

        // Act & Assert
        FluentActions.Invoking(() => new Graph(id, "Valid Title", "desc", defaultProjectId))
            .Should().Throw<ArgumentException>()
            .WithMessage("*projectId*");
    }

    /// <summary>
    /// Проверяет, что метод Update выбрасывает ArgumentException при передаче null, пустой строки или строки из пробелов в параметр newTitle.
    /// </summary>
    /// <param name="invalidTitle">Недопустимое значение нового заголовка.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void Update_Should_Throw_When_NewTitleIsNullOrWhitespace(string invalidTitle)
    {
        // Arrange
        var graph = new Graph(Guid.NewGuid(), "Valid Title", "desc", Guid.NewGuid());

        // Act & Assert
        FluentActions.Invoking(() => graph.Update(invalidTitle!, "new desc", Guid.NewGuid()))
            .Should().Throw<ArgumentException>()
            .WithMessage("*newTitle*");
    }

    /// <summary>
    /// Проверяет, что метод Update выбрасывает ArgumentException при передаче значения по умолчанию в параметр newProjectId.
    /// </summary>
    [Fact]
    public void Update_Should_Throw_When_NewProjectIdIsDefault()
    {
        // Arrange
        var graph = new Graph(Guid.NewGuid(), "Valid Title", "desc", Guid.NewGuid());
        var defaultProjectId = Guid.Empty;

        // Act & Assert
        FluentActions.Invoking(() => graph.Update("New Title", "new desc", defaultProjectId))
            .Should().Throw<ArgumentException>()
            .WithMessage("*newProjectId*");
    }

    /// <summary>
    /// Проверяет, что конструктор выбрасывает ValidationException при превышении максимальной длины заголовка.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowValidationException_When_TitleExceedsMaxLength()
    {
        // Arrange
        var id = Guid.NewGuid();
        var longTitle = new string('G', ValidationConstants.TitleMaxLength + 1);
        var projectId = Guid.NewGuid();

        // Act & Assert
        FluentActions.Invoking(() => new Graph(id, longTitle, "desc", projectId))
            .Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Title"));
    }
}