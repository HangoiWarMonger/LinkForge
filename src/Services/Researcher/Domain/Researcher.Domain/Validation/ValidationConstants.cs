namespace Researcher.Domain.Validation;

/// <summary>
/// Константы для ограничений валидации сущностей.
/// </summary>
public static class ValidationConstants
{
    /// <summary>
    /// Максимальная длина заголовка.
    /// </summary>
    public const int TitleMaxLength = 200;

    /// <summary>
    /// Максимальная длина описания.
    /// </summary>
    public const int DescriptionMaxLength = 1000;

    /// <summary>
    /// Максимальная глубина вложенности задач.
    /// </summary>
    public const int MaxDepth = 5;

    /// <summary>
    /// Максимальная длина имени.
    /// </summary>
    public const int NameMaxLength = 200;

    /// <summary>
    /// Максимальная длина типа.
    /// </summary>
    public const int TypeMaxLength = 100;

    /// <summary>
    /// Максимальное количество связей между двумя узлами.
    /// </summary>
    public const int MaxEdgesBetweenNodes = 2;

    /// <summary>
    /// Максимальная длина текста Markdown в документе.
    /// </summary>
    public const int BodyMarkdownMaxLength = 200000;
}