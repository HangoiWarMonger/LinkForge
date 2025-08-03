using System.ComponentModel.DataAnnotations;

namespace Researcher.Api.Common.Models;

/// <summary>
/// Запрос на обновление проекта.
/// </summary>
public record UpdateProjectRequest(
    [property: Required] string NewName,
    string? NewDescription);