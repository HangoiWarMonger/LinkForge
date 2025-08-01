namespace Researcher.Domain.Validation;

/// <summary>
/// Сообщения об ошибках валидации.
/// </summary>
public static class ValidationMessages
{
    /// <summary>
    /// Сообщение об обязательном указании заголовка.
    /// </summary>
    public const string TitleRequired = "Требуется указать заголовок.";

    /// <summary>
    /// Сообщение о максимальной длине заголовка.
    /// </summary>
    public static string TitleMaxLength =>
        $"Заголовок должен содержать не более {ValidationConstants.TitleMaxLength} символов.";

    /// <summary>
    /// Сообщение о максимальной длине описания.
    /// </summary>
    public static string DescriptionMaxLength =>
        $"Описание должно содержать не более {ValidationConstants.DescriptionMaxLength} символов.";

    /// <summary>
    /// Сообщение о недопустимом значении статуса.
    /// </summary>
    public const string InvalidStatus = "Недопустимое значение статуса.";

    /// <summary>
    /// Сообщение о превышении максимально допустимой глубины вложенности задачи.
    /// </summary>
    public static string DepthExceeded =>
        $"Глубина задачи не должна превышать {ValidationConstants.MaxDepth}.";

    /// <summary>
    /// Сообщение об обязательном указании имени.
    /// </summary>
    public const string NameRequired = "Требуется указать имя.";

    /// <summary>
    /// Сообщение о максимальной длине имени.
    /// </summary>
    public static string NameMaxLength =>
        $"Имя должно содержать не более {ValidationConstants.NameMaxLength} символов.";

    /// <summary>
    /// Сообщение об обязательном указании типа.
    /// </summary>
    public const string TypeRequired = "Требуется указать тип.";

    /// <summary>
    /// Сообщение о максимальной длине типа.
    /// </summary>
    public static string TypeMaxLength =>
        $"Тип должен содержать не более {ValidationConstants.TypeMaxLength} символов.";

    /// <summary>
    /// Сообщение о превышении максимального количества связей между двумя узлами.
    /// </summary>
    /// <param name="fromNodeId">Идентификатор исходного узла.</param>
    /// <param name="toNodeId">Идентификатор целевого узла.</param>
    /// <returns>Сообщение об ошибке.</returns>
    public static string MaxEdgesBetweenNodesExceeded(Guid fromNodeId, Guid toNodeId) =>
        $"Нельзя создать более {ValidationConstants.MaxEdgesBetweenNodes} связей между узлами {fromNodeId} и {toNodeId}.";

    /// <summary>
    /// Сообщение об обязательном указании исходного узла (FromNode).
    /// </summary>
    public const string FromNodeRequired = "Необходимо указать исходный узел (FromNode).";

    /// <summary>
    /// Сообщение об обязательном указании целевого узла (ToNode).
    /// </summary>
    public const string ToNodeRequired = "Необходимо указать целевой узел (ToNode).";

    /// <summary>
    /// Сообщение об обязательном указании идентификатора проекта (ProjectId).
    /// </summary>
    public const string ProjectIdRequired = "Требуется указать идентификатор проекта (ProjectId).";

    /// <summary>
    /// Сообщение об обязательном указании содержимого документа (BodyMarkdown).
    /// </summary>
    public const string BodyMarkdownRequired = "Требуется указать содержимое (BodyMarkdown).";

    /// <summary>
    /// Сообщение о максимальной длине содержимого документа (BodyMarkdown).
    /// </summary>
    public static string BodyMarkdownMaxLength =>
        $"Содержимое (BodyMarkdown) должно содержать не более {ValidationConstants.BodyMarkdownMaxLength} символов.";

    /// <summary>
    /// Сообщение об обязательном указании позиции.
    /// </summary>
    public const string PositionRequired = "Необходимо указать позицию.";

    /// <summary>
    /// Сообщение об обязательном указании идентификатора графа (GraphId).
    /// </summary>
    public const string GraphIdRequired = "Требуется указать идентификатор графа (GraphId).";
}