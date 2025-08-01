using FluentAssertions;
using Researcher.Domain.Entities;

namespace Researcher.Tests.Unit.Tests;

public class ProjectPositiveTests
{
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
            project.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

            project.Graphs.Should().NotBeNull().And.BeEmpty();
            project.Documents.Should().NotBeNull().And.BeEmpty();
            project.TaskItems.Should().NotBeNull().And.BeEmpty();
        }

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