using FluentAssertions;
using FluentValidation;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Tests.Unit.Tests;

/// <summary>
/// Набор негативных юнит-тестов для проверки конструктора и метода Update сущности Project на обработку некорректных данных.
/// </summary>
public class ProjectNegativeTests
{
    /// <summary>
    /// Проверяет, что конструктор выбрасывает ArgumentException при передаче null, пустой строки или строки из пробелов в параметр name.
    /// </summary>
    /// <param name="invalidName">Недопустимое значение имени.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void Constructor_Should_Throw_When_NameIsNullOrWhitespace(string invalidName)
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        FluentActions.Invoking(() => new Project(id, invalidName!, "desc"))
            .Should().Throw<ArgumentException>()
            .WithMessage("*name*");
    }

    /// <summary>
    /// Проверяет, что метод Update выбрасывает ArgumentException при передаче null, пустой строки или строки из пробелов в параметр newName.
    /// </summary>
    /// <param name="invalidName">Недопустимое значение нового имени.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void Update_Should_Throw_When_NewNameIsNullOrWhitespace(string invalidName)
    {
        // Arrange
        var project = new Project(Guid.NewGuid(), "Valid Name", "desc");

        // Act & Assert
        FluentActions.Invoking(() => project.Update(invalidName!, "new desc"))
            .Should().Throw<ArgumentException>()
            .WithMessage("*newName*");
    }

    /// <summary>
    /// Проверяет, что конструктор выбрасывает ValidationException при превышении максимальной длины имени.
    /// </summary>
    [Fact]
    public void Constructor_Should_ThrowValidationException_When_NameExceedsMaxLength()
    {
        // Arrange
        var id = Guid.NewGuid();
        var longName = new string('N', ValidationConstants.TitleMaxLength + 1);

        // Act & Assert
        FluentActions.Invoking(() => new Project(id, longName, "desc"))
            .Should().Throw<ValidationException>()
            .Where(e => e.Errors.Any(err => err.PropertyName == "Name"));
    }
}