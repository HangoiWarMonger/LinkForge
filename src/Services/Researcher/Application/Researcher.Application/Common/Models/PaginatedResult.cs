namespace Researcher.Application.Common.Models;

/// <summary>
/// Представляет результат пагинации с элементами и метаданными.
/// </summary>
/// <typeparam name="T">Тип элементов в результате.</typeparam>
/// <param name="Items">Список элементов на текущей странице.</param>
/// <param name="TotalCount">Общее количество элементов без учёта пагинации.</param>
/// <param name="Page">Номер текущей страницы (начинается с 1).</param>
/// <param name="PageSize">Размер страницы — количество элементов на странице.</param>
public record PaginatedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PageSize
);