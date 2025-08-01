using FluentAssertions;
using Researcher.Domain.Entities;

namespace Researcher.Tests.Unit.Tests;

public class GraphPositiveTests
{
    [Fact]
    public void Constructor_Should_CreateGraph_WithValidParameters()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Valid Graph Title";
        var description = "Some graph description";
        var projectId = Guid.NewGuid();

        // Act
        var graph = new Graph(id, title, description, projectId);

        // Assert
        graph.Should().NotBeNull();
        graph.Id.Should().Be(id);
        graph.Title.Should().Be(title);
        graph.Description.Should().Be(description);
        graph.ProjectId.Should().Be(projectId);

        graph.Nodes.Should().NotBeNull().And.BeEmpty();
        graph.Edges.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void Constructor_Should_CreateGraph_WithNullDescription()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Graph without description";
        var projectId = Guid.NewGuid();

        // Act
        var graph = new Graph(id, title, null, projectId);

        // Assert
        graph.Description.Should().BeNull();
    }

    [Fact]
    public void Update_Should_UpdateGraphProperties_WhenValidParameters()
    {
        // Arrange
        var graph = new Graph(Guid.NewGuid(), "Old Title", "Old Desc", Guid.NewGuid());
        var newTitle = "New Title";
        var newDescription = "New Desc";
        var newProjectId = Guid.NewGuid();

        // Act
        graph.Update(newTitle, newDescription, newProjectId);

        // Assert
        graph.Title.Should().Be(newTitle);
        graph.Description.Should().Be(newDescription);
        graph.ProjectId.Should().Be(newProjectId);
    }
}