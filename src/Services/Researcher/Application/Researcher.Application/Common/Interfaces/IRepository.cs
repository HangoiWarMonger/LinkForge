using System.Linq.Expressions;
using Researcher.Domain.Entities;

namespace Researcher.Application.Common.Interfaces;

/// <summary>
/// Общий интерфейс репозитория для CRUD-операций и гибких запросов.
/// </summary>
/// <typeparam name="T">Тип сущности.</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Получает сущность по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="trackChanges">Отслеживать изменения сущности (для обновления).</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task<T?> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает первый элемент, соответствующий условию, либо null.
    /// </summary>
    /// <param name="predicate">Условие фильтрации.</param>
    /// <param name="trackChanges">Отслеживать изменения сущности (для обновления).</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавляет новую сущность.
    /// </summary>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаляет сущность.
    /// </summary>
    Task RemoveAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Выполняет запрос с опциональными условиями фильтрации,
    /// проекции, сортировки и пагинации.
    /// </summary>
    /// <typeparam name="TResult">Тип проекции результата.</typeparam>
    /// <param name="predicate">Условие фильтрации.</param>
    /// <param name="selector">Проекционное выражение.</param>
    /// <param name="orderBy">Выражение сортировки.</param>
    /// <param name="ascending">Направление сортировки (true — по возрастанию).</param>
    /// <param name="skip">Пропустить указанное количество записей.</param>
    /// <param name="take">Взять указанное количество записей.</param>
    /// <param name="trackChanges">Отслеживать изменения сущности (для обновления).</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task<IEnumerable<TResult>> QueryAsync<TResult>(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, TResult>>? selector = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        int? skip = null,
        int? take = null,
        bool trackChanges = false,
        CancellationToken cancellationToken = default);
}