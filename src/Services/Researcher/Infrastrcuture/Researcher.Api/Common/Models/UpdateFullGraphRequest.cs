using System.ComponentModel.DataAnnotations;
using Researcher.Application.Common.Dto;

namespace Researcher.Api.Common.Models;

/// <summary>
/// Запрос на полное обновление графа с метаданными, узлами и ребрами.
/// </summary>
public record UpdateFullGraphRequest(
    [property: Required] string NewTitle,
    string? NewDescription,
    [property: Required] IReadOnlyList<NodeDto> Nodes,
    [property: Required] IReadOnlyList<EdgeDto> Edges);