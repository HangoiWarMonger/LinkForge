using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Infrastructure.DAL.EntityConfigurations;

/// <summary>
/// Конфигурация сущности <see cref="Graph"/> для Entity Framework Core.
/// </summary>
public class GraphConfiguration : IEntityTypeConfiguration<Graph>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Graph> builder)
    {
        builder.ToTable(nameof(Graph));

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .HasColumnName(nameof(Graph.Id))
            .IsRequired();

        builder.Property(g => g.Title)
            .HasColumnName(nameof(Graph.Title))
            .HasMaxLength(ValidationConstants.TitleMaxLength)
            .IsRequired();

        builder.Property(g => g.Description)
            .HasColumnName(nameof(Graph.Description))
            .HasMaxLength(ValidationConstants.DescriptionMaxLength)
            .IsRequired(false);

        builder.Property(g => g.ProjectId)
            .HasColumnName(nameof(Graph.ProjectId))
            .IsRequired();

        builder.Property(g => g.CreatedAtUtc)
            .HasColumnName(nameof(Graph.CreatedAtUtc))
            .IsRequired();

        builder.HasMany(g => g.Nodes)
            .WithOne()
            .HasForeignKey(n => n.GraphId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Edges)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}