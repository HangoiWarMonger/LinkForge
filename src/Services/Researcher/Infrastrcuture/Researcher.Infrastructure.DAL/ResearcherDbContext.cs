using Microsoft.EntityFrameworkCore;
using Researcher.Domain.Entities;

namespace Researcher.Infrastructure.DAL;

/// <summary>
/// Контекст базы данных для проекта Researcher.
/// </summary>
public class ResearcherDbContext : DbContext
{
    /// <summary>
    /// Набор проектов.
    /// </summary>
    public DbSet<Project> Projects { get; set; } = null!;

    /// <summary>
    /// Набор графов.
    /// </summary>
    public DbSet<Graph> Graphs { get; set; } = null!;

    /// <summary>
    /// Набор узлов графов.
    /// </summary>
    public DbSet<Node> Nodes { get; set; } = null!;

    /// <summary>
    /// Набор ребер графов.
    /// </summary>
    public DbSet<Edge> Edges { get; set; } = null!;

    /// <summary>
    /// Набор документов проектов.
    /// </summary>
    public DbSet<Document> Documents { get; set; } = null!;

    /// <summary>
    /// Набор задач проектов.
    /// </summary>
    public DbSet<TaskItem> TaskItems { get; set; } = null!;

    /// <summary>
    /// Создаёт новый экземпляр контекста базы данных ResearcherDbContext с указанными параметрами.
    /// </summary>
    /// <param name="options">Опции конфигурации контекста.</param>
    public ResearcherDbContext(DbContextOptions<ResearcherDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResearcherDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}