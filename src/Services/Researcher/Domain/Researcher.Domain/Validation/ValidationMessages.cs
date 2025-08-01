namespace Researcher.Domain.Validation;

public static class ValidationMessages
{
    public const string TitleRequired = "Title is required.";

    public static string TitleMaxLength =>
        $"Title must be at most {ValidationConstants.TitleMaxLength} characters.";

    public static string DescriptionMaxLength =>
        $"Description must be at most {ValidationConstants.DescriptionMaxLength} characters.";

    public const string InvalidStatus = "Invalid status value.";

    public static string DepthExceeded =>
        $"Task depth must not exceed {ValidationConstants.MaxDepth}.";

    public const string NameRequired = "Name is required.";

    public static string NameMaxLength =>
        $"Name must be at most {ValidationConstants.NameMaxLength} characters.";

    public const string TypeRequired = "Type is required.";

    public static string TypeMaxLength =>
        $"Type must be at most {ValidationConstants.TypeMaxLength} characters.";

    public static string MaxEdgesBetweenNodesExceeded(Guid fromNodeId, Guid toNodeId) =>
        $"Cannot create more than {ValidationConstants.MaxEdgesBetweenNodes} edges between nodes {fromNodeId} and {toNodeId}.";

    public const string FromNodeRequired = "FromNode must be provided.";
    public const string ToNodeRequired = "ToNode must be provided.";

    public const string ProjectIdRequired = "ProjectId is required.";

    public const string BodyMarkdownRequired = "BodyMarkdown is required.";

    public static string BodyMarkdownMaxLength =>
        $"BodyMarkdown must be at most {ValidationConstants.BodyMarkdownMaxLength} characters.";
}