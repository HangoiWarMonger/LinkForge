using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;
using Researcher.Domain.ValueObjects;

namespace Researcher.Infrastructure.DAL.EntityConfigurations;

/// <summary>
/// Конфигурация сущности <see cref="Node"/> для Entity Framework Core.
/// </summary>
public class NodeConfiguration : IEntityTypeConfiguration<Node>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Node> builder)
    {
        builder.ToTable(nameof(Node));

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .HasColumnName(nameof(Node.Id))
            .IsRequired();

        builder.Property(n => n.Title)
            .HasColumnName(nameof(Node.Title))
            .HasMaxLength(ValidationConstants.TitleMaxLength)
            .IsRequired();

        builder.Property(n => n.Description)
            .HasColumnName(nameof(Node.Description))
            .HasMaxLength(ValidationConstants.DescriptionMaxLength)
            .IsRequired(false);

        builder.Property(n => n.Type)
            .HasColumnName(nameof(Node.Type))
            .HasMaxLength(ValidationConstants.TypeMaxLength)
            .IsRequired();

        builder.Property(n => n.GraphId)
            .HasColumnName(nameof(Node.GraphId))
            .IsRequired();

        builder.Property(n => n.CreatedAtUtc)
            .HasColumnName(nameof(Node.CreatedAtUtc))
            .IsRequired();

        // Настройка ValueObject Position (если Position — ValueObject, его нужно конфигурировать отдельно)
        builder.OwnsOne(n => n.Position, pos =>
        {
            pos.Property(p => p.X)
                .HasColumnName(nameof(Position.X))
                .IsRequired();

            pos.Property(p => p.Y)
                .HasColumnName(nameof(Position.Y))
                .IsRequired();
        });

        builder.HasMany(n => n.OutgoingEdges)
            .WithOne(e => e.FromNode)
            .HasForeignKey(e => e.FromNodeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(n => n.IncomingEdges)
            .WithOne(e => e.ToNode)
            .HasForeignKey(e => e.ToNodeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}