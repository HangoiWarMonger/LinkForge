using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Infrastructure.DAL.EntityConfigurations;

/// <summary>
/// Конфигурация сущности <see cref="Document"/> для Entity Framework Core.
/// </summary>
public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable(nameof(Document));

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName(nameof(Document.Id))
            .IsRequired();

        builder.Property(d => d.Title)
            .HasColumnName(nameof(Document.Title))
            .HasMaxLength(ValidationConstants.TitleMaxLength)
            .IsRequired();

        builder.Property(d => d.BodyMarkdown)
            .HasColumnName(nameof(Document.BodyMarkdown))
            .HasMaxLength(ValidationConstants.BodyMarkdownMaxLength)
            .IsRequired();

        builder.Property(d => d.IsInternal)
            .HasColumnName(nameof(Document.IsInternal))
            .IsRequired();

        builder.Property(d => d.ProjectId)
            .HasColumnName(nameof(Document.ProjectId))
            .IsRequired();

        builder.Property(d => d.UpdatedAt)
            .HasColumnName(nameof(Document.UpdatedAt))
            .IsRequired();

        builder.Property(d => d.CreatedAtUtc)
            .HasColumnName(nameof(Document.CreatedAtUtc))
            .IsRequired();

        builder.HasOne(d => d.Project)
            .WithMany(p => p.Documents)
            .HasForeignKey(d => d.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}