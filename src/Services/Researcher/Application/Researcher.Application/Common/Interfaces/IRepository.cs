using System.Linq.Expressions;
using Researcher.Application.Common.Models;
using Researcher.Domain.Entities;

namespace Researcher.Application.Common.Interfaces;

/// <summary>
/// Общий интерфейс репозитория для CRUD-операций и гибких запросов.
/// </summary>
/// <typeparam name="T">Тип сущности.</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Получить сущность по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="trackChanges">Отслеживать изменения для обновления.</param>
    /// <param name="includes">Связанные сущности для жадной загрузки.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Сущность или null, если не найдена.</returns>
    Task<T?> GetByIdAsync(
        Guid id,
        bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes);

    /// <summary>
    /// Получить первый элемент по условию или null.
    /// </summary>
    /// <param name="predicate">Условие фильтрации.</param>
    /// <param name="trackChanges">Отслеживать изменения для обновления.</param>
    /// <param name="includes">Связанные сущности для жадной загрузки.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes);

    /// <summary>
    /// Добавить новую сущность.
    /// </summary>
    /// <param name="entity">Сущность для добавления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить сущность.
    /// </summary>
    /// <param name="entity">Сущность для удаления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task RemoveAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Выполнить запрос с фильтрацией, сортировкой, пагинацией и загрузкой связей.
    /// </summary>
    /// <param name="predicate">Условие фильтрации.</param>
    /// <param name="orderBy">Выражение для сортировки.</param>
    /// <param name="ascending">Направление сортировки (true — по возрастанию).</param>
    /// <param name="page">Номер страницы, начиная с 1.</param>
    /// <param name="pageSize">Размер страницы.</param>
    /// <param name="trackChanges">Отслеживать изменения для обновления.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <param name="includes">Связанные сущности для жадной загрузки.</param>
    /// <returns>Пагинированный результат.</returns>
    Task<PaginatedResult<T>> QueryAsync(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        int? page = null,
        int? pageSize = null,
        bool trackChanges = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes);

    /// <summary>
    /// Выполнить запрос с проекцией, фильтрацией, сортировкой и пагинацией.
    /// </summary>
    /// <typeparam name="TResult">Тип результата проекции.</typeparam>
    /// <param name="predicate">Условие фильтрации.</param>
    /// <param name="selector">Выражение проекции.</param>
    /// <param name="orderBy">Выражение для сортировки.</param>
    /// <param name="ascending">Направление сортировки (true — по возрастанию).</param>
    /// <param name="page">Номер страницы, начиная с 1.</param>
    /// <param name="pageSize">Размер страницы.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <param name="includes">Связанные сущности для жадной загрузки.</param>
    /// <returns>Пагинированный результат проекции.</returns>
    Task<PaginatedResult<TResult>> QueryAsync<TResult>(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, TResult>>? selector = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        int? page = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includes);
}