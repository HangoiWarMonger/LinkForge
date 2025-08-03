namespace Researcher.Application.Common.Dto;

/// <summary>
/// DTO позиции узла.
/// </summary>
/// <param name="X">Координата X.</param>
/// <param name="Y">Координата Y.</param>
public record PositionDto(
    double X,
    double Y
);