using System.Linq.Expressions;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Researcher.Application.Common.Interfaces;
using Researcher.Application.Common.Models;
using Researcher.Domain.Entities;

namespace Researcher.Infrastructure.DAL.Services;

/// <summary>
/// Репозиторий для работы с сущностями типа <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">Тип сущности, наследующей <see cref="BaseEntity"/>.</typeparam>
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet;

    /// <summary>
    /// Создаёт новый экземпляр репозитория.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    public Repository(ResearcherDbContext dbContext)
    {
        Guard.Against.Null(dbContext);
        _dbSet = dbContext.Set<T>();
    }

    /// <inheritdoc />
    public async Task<T?> GetByIdAsync(
        Guid id,
        bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        Guard.Against.Default(id);
        var query = BuildQuery(trackChanges, e => e.Id == id, includes);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        Guard.Against.Null(predicate);
        var query = BuildQuery(trackChanges, predicate, includes);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entity);
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc />
    public Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entity);
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<T>> QueryAsync(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        int? page = null,
        int? pageSize = null,
        bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        var query = BuildQuery(trackChanges, predicate, includes);

        if (orderBy != null)
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

        var totalCount = await query.CountAsync(cancellationToken);

        if (page.HasValue && pageSize.HasValue)
        {
            var skip = (page.Value - 1) * pageSize.Value;
            query = query.Skip(skip).Take(pageSize.Value);
        }

        var items = await query.ToListAsync(cancellationToken);

        return new PaginatedResult<T>(items, totalCount, page ?? 1, pageSize ?? totalCount);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<TResult>> QueryAsync<TResult>(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, TResult>>? selector = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        int? page = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes)
    {
        Guard.Against.Null(selector);
        var query = BuildQuery(false, predicate, includes); // trackChanges для проекции можно всегда false

        if (orderBy != null)
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

        var totalCount = await query.CountAsync(cancellationToken);

        if (page.HasValue && pageSize.HasValue)
        {
            var skip = (page.Value - 1) * pageSize.Value;
            query = query.Skip(skip).Take(pageSize.Value);
        }

        var projected = query.Select(selector);

        var items = await projected.ToListAsync(cancellationToken);

        return new PaginatedResult<TResult>(items, totalCount, page ?? 1, pageSize ?? totalCount);
    }

    private IQueryable<T> BuildQuery(
        bool trackChanges = false,
        Expression<Func<T, bool>>? predicate = null,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        if (!trackChanges)
            query = query.AsNoTracking();

        foreach (var include in includes)
            query = query.Include(include);

        if (predicate != null)
            query = query.Where(predicate);

        return query;
    }
}