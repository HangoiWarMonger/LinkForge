using System.Collections;

namespace Researcher.Api.Common.Models;

/// <summary>
/// Представляет стандартный формат ошибки для ответа API.
/// </summary>
public record ErrorResponse
{
    /// <summary>
    /// Тип исключения или ошибки.
    /// </summary>
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Стек вызовов ошибки (если доступен).
    /// </summary>
    public string? StackTrace { get; init; }

    /// <summary>
    /// Дополнительные данные ошибки.
    /// </summary>
    public IDictionary? Data { get; init; }
}