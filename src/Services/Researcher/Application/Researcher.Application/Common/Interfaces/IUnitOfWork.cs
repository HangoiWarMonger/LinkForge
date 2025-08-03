namespace Researcher.Application.Common.Interfaces;

/// <summary>
/// Единица работы для управления сохранением изменений.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Сохраняет все изменения в хранилище данных.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}