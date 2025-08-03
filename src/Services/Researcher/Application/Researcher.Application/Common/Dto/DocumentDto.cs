namespace Researcher.Application.Common.Dto;

/// <summary>
/// DTO документа проекта.
/// </summary>
/// <param name="Id">Идентификатор документа.</param>
/// <param name="Title">Заголовок документа.</param>
/// <param name="BodyMarkdown">Содержимое в формате Markdown.</param>
/// <param name="IsInternal">Признак внутреннего документа.</param>
/// <param name="UpdatedAt">Дата последнего обновления.</param>
/// <param name="ProjectId">Идентификатор проекта.</param>
/// <param name="CreatedAtUtc">Дата создания.</param>
public record DocumentDto(
    Guid Id,
    string Title,
    string BodyMarkdown,
    bool IsInternal,
    DateTimeOffset UpdatedAt,
    Guid ProjectId,
    DateTimeOffset CreatedAtUtc
); 