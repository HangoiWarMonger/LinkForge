using FluentAssertions;
using Researcher.Domain.Entities;
using Researcher.Domain.ValueObjects;

namespace Researcher.Tests.Unit.Tests;

  public class EdgePositiveTests
    {
       private static Node CreateNode(Guid? id = null)
        {
            return new Node(
                id ?? Guid.NewGuid(),
                "Node Title",
                "Some description",
                "NodeType",
                new Position(1.0, 2.0),
                Guid.NewGuid());
        }

        [Fact]
        public void Constructor_Should_CreateEdge_AndAttachToNodes()
        {
            // Arrange
            var fromNode = CreateNode();
            var toNode = CreateNode();

            var id = Guid.NewGuid();
            var type = "Relation";
            string? description = "Edge description";

            // Act
            var edge = new Edge(id, type, description, fromNode, toNode);

            // Assert
            edge.Should().NotBeNull();
            edge.Id.Should().Be(id);
            edge.Type.Should().Be(type);
            edge.Description.Should().Be(description);
            edge.FromNode.Should().BeSameAs(fromNode);
            edge.ToNode.Should().BeSameAs(toNode);
            edge.FromNodeId.Should().Be(fromNode.Id);
            edge.ToNodeId.Should().Be(toNode.Id);

            fromNode.OutgoingEdges.Should().Contain(edge);
            toNode.IncomingEdges.Should().Contain(edge);
        }

        [Fact]
        public void Update_Should_UpdateProperties_AndReattachNodes()
        {
            // Arrange
            var fromNode1 = CreateNode();
            var toNode1 = CreateNode();

            var fromNode2 = CreateNode();
            var toNode2 = CreateNode();

            var edge = new Edge(Guid.NewGuid(), "OldType", "OldDesc", fromNode1, toNode1);

            var newType = "NewType";
            string? newDescription = "NewDesc";

            // Act
            edge.Update(newType, newDescription, fromNode2, toNode2);

            // Assert
            edge.Type.Should().Be(newType);
            edge.Description.Should().Be(newDescription);
            edge.FromNode.Should().BeSameAs(fromNode2);
            edge.ToNode.Should().BeSameAs(toNode2);
            edge.FromNodeId.Should().Be(fromNode2.Id);
            edge.ToNodeId.Should().Be(toNode2.Id);

            fromNode1.OutgoingEdges.Should().NotContain(edge);
            toNode1.IncomingEdges.Should().NotContain(edge);

            fromNode2.OutgoingEdges.Should().Contain(edge);
            toNode2.IncomingEdges.Should().Contain(edge);
        }
    }