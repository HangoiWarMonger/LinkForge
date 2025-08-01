using System.ComponentModel;
using FluentAssertions;
using FluentValidation;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;
using Researcher.Domain.ValueObjects;

namespace Researcher.Tests.Unit.Tests;

/// <summary>
/// Набор негативных юнит-тестов для проверки конструктора и метода Update сущности TaskItem на обработку некорректных данных.
/// </summary>
public class TaskItemNegativeTests
{
    /// <summary>
    /// Проверяет, что конструктор выбрасывает ArgumentException при передаче null, пустой строки или строки из пробелов в параметр title.
    /// </summary>
    /// <param name="invalidTitle">Недопустимое значение заголовка.</param>
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

    /// <summary>
    /// Проверяет, что конструктор выбрасывает ArgumentException при передаче значения по умолчанию в параметр projectId.
    /// </summary>
    [Fact]
    public void Constructor_Should_Throw_When_ProjectIdIsDefault()
    {
        // Arrange
        var defaultProjectId = Guid.Empty;

        // Act & Assert
        FluentActions.Invoking(() => new TaskItem(Guid.NewGuid(), "Valid Title", null, TaskItemStatus.Todo, defaultProjectId))
            .Should().Throw<ArgumentException>()
            .WithMessage("*projectId*");
    }

    /// <summary>
    /// Проверяет, что конструктор выбрасывает InvalidEnumArgumentException при передаче недопустимого значения в параметр status.
    /// </summary>
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

    /// <summary>
    /// Проверяет, что метод Update выбрасывает ArgumentException при передаче null, пустой строки или строки из пробелов в параметр newTitle.
    /// </summary>
    /// <param name="invalidTitle">Недопустимое значение нового заголовка.</param>
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

    /// <summary>
    /// Проверяет, что метод Update выбрасывает InvalidEnumArgumentException при передаче недопустимого значения в параметр newStatus.
    /// </summary>
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

    /// <summary>
    /// Проверяет, что конструктор выбрасывает ValidationException при превышении максимальной длины заголовка.
    /// </summary>
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

    /// <summary>
    /// Проверяет, что конструктор выбрасывает ValidationException при превышении максимальной длины описания.
    /// </summary>
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

    /// <summary>
    /// Проверяет, что конструктор выбрасывает ValidationException при превышении максимальной глубины вложенности задач.
    /// </summary>
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