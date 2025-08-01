using FluentAssertions;
using FluentValidation;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Tests.Unit.Tests;

public class DocumentNegativeTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_Should_Throw_When_TitleIsNullOrWhitespace(string invalidTitle)
    {
        // Arrange
        var id = Guid.NewGuid();
        var body = "Valid body";
        var isInternal = true;
        var projectId = Guid.NewGuid();

        // Act & Assert
        FluentActions.Invoking(() => new Document(id, invalidTitle!, body, isInternal, projectId))
            .Should().Throw<ArgumentException>()
            .WithMessage("*title*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_Should_Throw_When_BodyMarkdownIsNullOrWhitespace(string invalidBody)
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Valid Title";
        var isInternal = true;
        var projectId = Guid.NewGuid();

        // Act & Assert
        FluentActions.Invoking(() => new Document(id, title, invalidBody!, isInternal, projectId))
            .Should().Throw<ArgumentException>()
            .WithMessage("*bodyMarkdown*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_ProjectIdIsDefault()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Valid Title";
        var body = "Valid body";
        var isInternal = true;
        var projectId = default(Guid);

        // Act & Assert
        FluentActions.Invoking(() => new Document(id, title, body, isInternal, projectId))
            .Should().Throw<ArgumentException>()
            .WithMessage("*projectId*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_Should_Throw_When_NewTitleIsNullOrWhitespace(string invalidTitle)
    {
        // Arrange
        var document = new Document(
            Guid.NewGuid(),
            "Initial Title",
            "Initial body",
            true,
            Guid.NewGuid());

        var newBody = "New body";

        // Act & Assert
        FluentActions.Invoking(() => document.Update(invalidTitle!, newBody, true))
            .Should().Throw<ArgumentException>()
            .WithMessage("*newTitle*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_Should_Throw_When_NewBodyMarkdownIsNullOrWhitespace(string invalidBody)
    {
        // Arrange
        var document = new Document(
            Guid.NewGuid(),
            "Initial Title",
            "Initial body",
            true,
            Guid.NewGuid());

        var newTitle = "New Title";

        // Act & Assert
        FluentActions.Invoking(() => document.Update(newTitle, invalidBody!, false))
            .Should().Throw<ArgumentException>()
            .WithMessage("*newBodyMarkdown*");
    }

    [Fact]
    public void Constructor_Should_ThrowValidationException_When_TitleExceedsMaxLength()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = new string('A', ValidationConstants.TitleMaxLength + 1);
        var body = "Valid body";
        var isInternal = true;
        var projectId = Guid.NewGuid();

        // Act & Assert
        FluentActions.Invoking(() => new Document(id, title, body, isInternal, projectId))
            .Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Title"));
    }

    [Fact]
    public void Constructor_Should_ThrowValidationException_When_BodyMarkdownExceedsMaxLength()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Valid Title";
        var body = new string('B', ValidationConstants.BodyMarkdownMaxLength + 1);
        var isInternal = true;
        var projectId = Guid.NewGuid();

        // Act & Assert
        FluentActions.Invoking(() => new Document(id, title, body, isInternal, projectId))
            .Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "BodyMarkdown"));
    }
}