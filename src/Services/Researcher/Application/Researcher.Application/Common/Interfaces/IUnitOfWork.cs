namespace Researcher.Application.Common.Interfaces;

/// <summary>
/// Интерфейс единицы работы (Unit of Work) для управления транзакциями и сохранением изменений.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Асинхронно сохраняет все изменения в хранилище данных.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}