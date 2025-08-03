using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Researcher.Domain.Exceptions;

namespace Researcher.Domain.Validation;

/// <summary>
/// Расширения для выбрасывания специфичных исключений при проверках.
/// </summary>
public static class ThrowIf
{
    /// <summary>
    /// Проверяет, что сущность не равна null. Если null — бросает EntityNotFoundException.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <param name="input">Проверяемая сущность.</param>
    /// <param name="id">Идентификатор самой сущности (для сообщения).</param>
    /// <param name="parameterName">Имя параметра (автоматически).</param>
    /// <returns>Непустая сущность.</returns>
    public static T EntityIsNull<T>(
        [NotNull] T? input,
        Guid id,
        [CallerArgumentExpression("input")] string? parameterName = null)
        where T : class
    {
        if (input is null)
            throw new EntityNotFoundException(typeof(T).Name, id);

        return input;
    }
}