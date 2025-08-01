using FluentAssertions;
using Researcher.Domain.Entities;

namespace Researcher.Tests.Unit.Tests;

/// <summary>
/// Набор позитивных юнит-тестов для проверки создания и обновления сущности Project.
/// </summary>
public class ProjectPositiveTests
{
    /// <summary>
    /// Проверяет, что конструктор создаёт объект Project с корректными параметрами.
    /// </summary>
    [Fact]
    public void Constructor_Should_CreateProject_WithValidParameters()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Valid Project Name";
        var description = "Some description";

        // Act
        var project = new Project(id, name, description);

        // Assert
        project.Should().NotBeNull();
        project.Id.Should().Be(id);
        project.Name.Should().Be(name);
        project.Description.Should().Be(description);
        project.CreatedAtUtc.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));

        project.Graphs.Should().NotBeNull().And.BeEmpty();
        project.Documents.Should().NotBeNull().And.BeEmpty();
        project.TaskItems.Should().NotBeNull().And.BeEmpty();
    }

    /// <summary>
    /// Проверяет, что конструктор создаёт объект Project с null в описании.
    /// </summary>
    [Fact]
    public void Constructor_Should_CreateProject_WithNullDescription()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Project without description";

        // Act
        var project = new Project(id, name, null);

        // Assert
        project.Description.Should().BeNull();
    }

    /// <summary>
    /// Проверяет, что метод Update корректно обновляет имя и описание проекта при валидных параметрах.
    /// </summary>
    [Fact]
    public void Update_Should_UpdateNameAndDescription_WhenValidParameters()
    {
        // Arrange
        var project = new Project(Guid.NewGuid(), "Old Name", "Old Description");
        var newName = "New Name";
        var newDescription = "New Description";

        // Act
        project.Update(newName, newDescription);

        // Assert
        project.Name.Should().Be(newName);
        project.Description.Should().Be(newDescription);
    }

    /// <summary>
    /// Проверяет, что метод Update допускает установку null в описание проекта.
    /// </summary>
    [Fact]
    public void Update_Should_Allow_NullDescription()
    {
        // Arrange
        var project = new Project(Guid.NewGuid(), "Old Name", "Old Description");

        // Act
        project.Update("New Name", null);

        // Assert
        project.Description.Should().BeNull();
    }
}