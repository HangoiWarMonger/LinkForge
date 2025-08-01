using FluentAssertions;
using Researcher.Domain.Entities;
using Researcher.Domain.ValueObjects;

namespace Researcher.Tests.Unit.Tests;

public class TaskItemPositiveTests
{
    [Fact]
    public void Constructor_Should_CreateTaskItem_WithValidParameters_AndSetParentChildRelation()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var parent = new TaskItem(
            Guid.NewGuid(),
            "Parent task",
            "Parent description",
            TaskItemStatus.Todo,
            projectId);

        var id = Guid.NewGuid();
        var title = "Child task";
        var description = "Child description";
        var status = TaskItemStatus.Done;

        // Act
        var child = new TaskItem(id, title, description, status, projectId, parent);

        // Assert
        child.Should().NotBeNull();
        child.Id.Should().Be(id);
        child.Title.Should().Be(title);
        child.Description.Should().Be(description);
        child.Status.Should().Be(status);
        child.ProjectId.Should().Be(projectId);
        child.Parent.Should().Be(parent);
        child.ParentId.Should().Be(parent.Id);
        parent.Children.Should().Contain(child);
        child.GetDepth().Should().Be(parent.GetDepth() + 1);
    }

    [Fact]
    public void Constructor_Should_CreateRootTask_WhenParentIsNull()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        // Act
        var task = new TaskItem(
            Guid.NewGuid(),
            "Root Task",
            null,
            TaskItemStatus.Todo,
            projectId,
            parent: null);

        // Assert
        task.Parent.Should().BeNull();
        task.ParentId.Should().BeNull();
        task.Children.Should().BeEmpty();
        task.GetDepth().Should().Be(1);
    }

    [Fact]
    public void Update_Should_UpdateProperties_AndChangeParentProperly()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var oldParent = new TaskItem(Guid.NewGuid(), "Old Parent", null, TaskItemStatus.Todo, projectId);
        var newParent = new TaskItem(Guid.NewGuid(), "New Parent", null, TaskItemStatus.Todo, projectId);

        var task = new TaskItem(Guid.NewGuid(), "Task", "Desc", TaskItemStatus.Todo, projectId, oldParent);

        var newTitle = "Updated Title";
        var newDescription = "Updated Desc";
        var newStatus = TaskItemStatus.Done;

        // Act
        task.Update(newTitle, newDescription, newStatus, newParent);

        // Assert
        task.Title.Should().Be(newTitle);
        task.Description.Should().Be(newDescription);
        task.Status.Should().Be(newStatus);

        // Проверяем смену родителя
        task.Parent.Should().Be(newParent);
        task.ParentId.Should().Be(newParent.Id);

        oldParent.Children.Should().NotContain(task);
        newParent.Children.Should().Contain(task);

        task.GetDepth().Should().Be(newParent.GetDepth() + 1);
    }
}