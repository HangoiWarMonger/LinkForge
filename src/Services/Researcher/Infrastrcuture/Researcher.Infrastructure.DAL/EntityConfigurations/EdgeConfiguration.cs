using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Infrastructure.DAL.EntityConfigurations;

/// <summary>
/// Конфигурация сущности <see cref="Edge"/> для Entity Framework Core.
/// </summary>
public class EdgeConfiguration : IEntityTypeConfiguration<Edge>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Edge> builder)
    {
        builder.ToTable(nameof(Edge));

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName(nameof(Edge.Id))
            .IsRequired();

        builder.Property(e => e.Type)
            .HasColumnName(nameof(Edge.Type))
            .HasMaxLength(ValidationConstants.TypeMaxLength)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnName(nameof(Edge.Description))
            .HasMaxLength(ValidationConstants.DescriptionMaxLength)
            .IsRequired(false);

        builder.Property(e => e.FromNodeId)
            .HasColumnName(nameof(Edge.FromNodeId))
            .IsRequired();

        builder.Property(e => e.ToNodeId)
            .HasColumnName(nameof(Edge.ToNodeId))
            .IsRequired();

        builder.Property(e => e.CreatedAtUtc)
            .HasColumnName(nameof(Edge.CreatedAtUtc))
            .IsRequired();

        // Навигации к узлам настраиваем в Node конфигурации
        builder.HasIndex(e => e.FromNodeId);
        builder.HasIndex(e => e.ToNodeId);
    }
}