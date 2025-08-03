using System.ComponentModel.DataAnnotations;

namespace Researcher.Api.Common.Models;

/// <summary>
/// Запрос на обновление документа.
/// </summary>
public record UpdateDocumentRequest(
    [property: Required] string NewTitle,
    [property: Required] string NewBodyMarkdown,
    bool NewIsInternal);