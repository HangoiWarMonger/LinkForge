using System.ComponentModel;
using FluentAssertions;
using FluentValidation;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;
using Researcher.Domain.ValueObjects;

namespace Researcher.Tests.Unit.Tests;

public class TaskItemNegativeTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_Should_Throw_When_TitleIsNullOrWhitespace(string invalidTitle)
    {
        // Arrange
        var projectId = Guid.NewGuid();

        // Act & Assert
        FluentActions.Invoking(() => new TaskItem(Guid.NewGuid(), invalidTitle!, null, TaskItemStatus.Todo, projectId))
            .Should().Throw<ArgumentException>()
            .WithMessage("*title*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_ProjectIdIsDefault()
    {
        // Arrange
        var defaultProjectId = default(Guid);

        // Act & Assert
        FluentActions.Invoking(() => new TaskItem(Guid.NewGuid(), "Valid Title", null, TaskItemStatus.Todo, defaultProjectId))
            .Should().Throw<ArgumentException>()
            .WithMessage("*projectId*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_StatusIsOutOfRange()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var invalidStatus = (TaskItemStatus) 999;

        // Act & Assert
        FluentActions.Invoking(() => new TaskItem(Guid.NewGuid(), "Valid Title", null, invalidStatus, projectId))
            .Should().Throw<InvalidEnumArgumentException>()
            .WithMessage("*status*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_Should_Throw_When_NewTitleIsNullOrWhitespace(string invalidTitle)
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var task = new TaskItem(Guid.NewGuid(), "Initial", null, TaskItemStatus.Todo, projectId);

        // Act & Assert
        FluentActions.Invoking(() => task.Update(invalidTitle!, "desc", TaskItemStatus.Done))
            .Should().Throw<ArgumentException>()
            .WithMessage("*newTitle*");
    }

    [Fact]
    public void Update_Should_Throw_When_NewStatusIsOutOfRange()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var task = new TaskItem(Guid.NewGuid(), "Initial", null, TaskItemStatus.Todo, projectId);
        var invalidStatus = (TaskItemStatus) 999;

        // Act & Assert
        FluentActions.Invoking(() => task.Update("New Title", "desc", invalidStatus))
            .Should().Throw<InvalidEnumArgumentException>()
            .WithMessage("*newStatus*");
    }

    [Fact]
    public void Constructor_Should_ThrowValidationException_When_TitleExceedsMaxLength()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var title = new string('A', ValidationConstants.TitleMaxLength + 1);

        // Act & Assert
        FluentActions.Invoking(() => new TaskItem(Guid.NewGuid(), title, null, TaskItemStatus.Todo, projectId))
            .Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Title"));
    }

    [Fact]
    public void Constructor_Should_ThrowValidationException_When_DescriptionExceedsMaxLength()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var description = new string('D', ValidationConstants.DescriptionMaxLength + 1);

        // Act & Assert
        FluentActions.Invoking(() => new TaskItem(Guid.NewGuid(), "Valid Title", description, TaskItemStatus.Todo, projectId))
            .Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Description"));
    }

    [Fact]
    public void Constructor_Should_ThrowValidationException_When_DepthExceedsMax()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        // Создаём цепочку задач глубже максимальной (MaxDepth + 1)
        TaskItem? parent = null;
        for (int i = 0; i < ValidationConstants.MaxDepth; i++)
        {
            parent = new TaskItem(Guid.NewGuid(), $"Task {i}", null, TaskItemStatus.Todo, projectId, parent);
        }

        // Act & Assert - создание следующего уровня превысит глубину
        FluentActions.Invoking(() => new TaskItem(Guid.NewGuid(), "Too Deep Task", null, TaskItemStatus.Todo, projectId, parent))
            .Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == ""));
    }
}