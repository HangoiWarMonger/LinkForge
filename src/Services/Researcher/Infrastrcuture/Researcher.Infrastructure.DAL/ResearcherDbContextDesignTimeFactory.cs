using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Researcher.Domain.ValueObjects;

namespace Researcher.Infrastructure.DAL;

/// <summary>
/// Фабрика для создания контекста базы данных в режиме дизайна (миграции и др.).
/// </summary>
public class ResearcherDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ResearcherDbContext>
{
    /// <inheritdoc />
    public ResearcherDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ResearcherDbContext>();
        optionsBuilder.UseNpgsql(npgsqlOptions =>
        {
            // Регистрируем enum для миграций
            npgsqlOptions.MapEnum<TaskItemStatus>();
        });
        return new ResearcherDbContext(optionsBuilder.Options);
    }
}