using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Infrastructure.DAL.EntityConfigurations;

/// <summary>
/// Конфигурация сущности <see cref="TaskItem"/> для Entity Framework Core.
/// </summary>
public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable(nameof(TaskItem));

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName(nameof(TaskItem.Id))
            .IsRequired();

        builder.Property(t => t.Title)
            .HasColumnName(nameof(TaskItem.Title))
            .HasMaxLength(ValidationConstants.TitleMaxLength)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasColumnName(nameof(TaskItem.Description))
            .HasMaxLength(ValidationConstants.DescriptionMaxLength)
            .IsRequired(false);

        builder.Property(t => t.Status)
            .HasColumnName(nameof(TaskItem.Status))
            .IsRequired();

        builder.Property(t => t.ProjectId)
            .HasColumnName(nameof(TaskItem.ProjectId))
            .IsRequired();

        builder.Property(t => t.ParentId)
            .HasColumnName(nameof(TaskItem.ParentId))
            .IsRequired(false);

        builder.Property(t => t.CreatedAtUtc)
            .HasColumnName(nameof(TaskItem.CreatedAtUtc))
            .IsRequired();

        builder.HasOne(t => t.Parent)
            .WithMany(t => t.Children)
            .HasForeignKey(t => t.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Children)
            .WithOne(t => t.Parent)
            .HasForeignKey(t => t.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}