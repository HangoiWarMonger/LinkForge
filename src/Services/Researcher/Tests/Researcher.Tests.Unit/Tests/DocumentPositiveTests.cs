using FluentAssertions;
using Researcher.Domain.Entities;

namespace Researcher.Tests.Unit.Tests;

/// <summary>
/// Набор позитивных юнит-тестов для проверки создания и обновления сущности Document.
/// </summary>
public class DocumentPositiveTests
{
    /// <summary>
    /// Проверяет, что конструктор успешно создаёт объект Document при корректных параметрах.
    /// </summary>
    [Fact]
    public void Constructor_Should_CreateDocument_When_ValidParameters()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Valid Title";
        var body = "Valid **Markdown** content";
        var isInternal = true;
        var projectId = Guid.NewGuid();

        // Act
        var document = new Document(id, title, body, isInternal, projectId);

        // Assert
        document.Should().NotBeNull();
        document.Id.Should().Be(id);
        document.Title.Should().Be(title);
        document.BodyMarkdown.Should().Be(body);
        document.IsInternal.Should().Be(isInternal);
        document.ProjectId.Should().Be(projectId);
        document.CreatedAtUtc.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
        document.UpdatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Проверяет, что метод Update корректно обновляет свойства сущности при валидных параметрах.
    /// </summary>
    [Fact]
    public void Update_Should_UpdateProperties_When_ValidParameters()
    {
        // Arrange
        var document = new Document(
            Guid.NewGuid(),
            "Initial Title",
            "Initial body",
            true,
            Guid.NewGuid());

        var newTitle = "Updated Title";
        var newBody = "Updated body markdown";
        var newIsInternal = false;

        // Act
        document.Update(newTitle, newBody, newIsInternal);

        // Assert
        document.Title.Should().Be(newTitle);
        document.BodyMarkdown.Should().Be(newBody);
        document.IsInternal.Should().Be(newIsInternal);
        document.UpdatedAt.Should().BeAfter(document.CreatedAtUtc);
    }
}