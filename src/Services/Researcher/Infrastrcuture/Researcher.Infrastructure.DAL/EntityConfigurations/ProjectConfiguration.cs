using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Infrastructure.DAL.EntityConfigurations;

/// <summary>
/// Конфигурация сущности <see cref="Project"/> для Entity Framework Core.
/// </summary>
public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable(nameof(Project));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName(nameof(Project.Id))
            .IsRequired();

        builder.Property(p => p.Name)
            .HasColumnName(nameof(Project.Name))
            .HasMaxLength(ValidationConstants.NameMaxLength)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnName(nameof(Project.Description))
            .HasMaxLength(ValidationConstants.DescriptionMaxLength)
            .IsRequired(false);

        builder.Property(p => p.CreatedAtUtc)
            .HasColumnName(nameof(Project.CreatedAtUtc))
            .IsRequired();

        // Навигации (при необходимости можно настроить)
        builder.HasMany(p => p.Graphs)
            .WithOne()
            .HasForeignKey(g => g.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Documents)
            .WithOne(d => d.Project)
            .HasForeignKey(d => d.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.TaskItems)
            .WithOne()
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}