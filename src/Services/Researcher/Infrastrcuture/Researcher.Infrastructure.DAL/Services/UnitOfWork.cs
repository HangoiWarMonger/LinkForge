using Ardalis.GuardClauses;
using Researcher.Application.Common.Interfaces;

namespace Researcher.Infrastructure.DAL.Services;

/// <summary>
/// Единица работы для управления сохранением изменений в базе данных.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ResearcherDbContext _dbContext;

    /// <summary>
    /// Создает экземпляр UnitOfWork с указанным контекстом базы данных.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    /// <exception cref="ArgumentNullException">Если <paramref name="dbContext"/> равен null.</exception>
    public UnitOfWork(ResearcherDbContext dbContext)
    {
        _dbContext = Guard.Against.Null(dbContext);
    }

    /// <inheritdoc />
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}