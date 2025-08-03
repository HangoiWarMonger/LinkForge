using Mapster;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Models;
using Researcher.Domain.Entities;
using Researcher.Domain.ValueObjects;

namespace Researcher.Application.Common.Mappings;

/// <summary>
/// Регистрация всех маппингов между доменными моделями и DTO.
/// </summary>
public class MappingProfile : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
         // Project → ProjectDto
        config.NewConfig<Project, ProjectDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.CreatedAtUtc, src => src.CreatedAtUtc);

        // Graph → GraphDto
        config.NewConfig<Graph, GraphDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.ProjectId, src => src.ProjectId)
            .Map(dest => dest.CreatedAtUtc, src => src.CreatedAtUtc);

        // Graph → FullGraphDto (оставляем как есть, если там другой конфиг, можно явно тоже сделать)

        // Position → PositionDto
        config.NewConfig<Position, PositionDto>()
            .Map(dest => dest.X, src => src.X)
            .Map(dest => dest.Y, src => src.Y);

        // Node → NodeDto
        config.NewConfig<Node, NodeDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.Position, src => src.Position) // вызовет Position → PositionDto
            .Map(dest => dest.GraphId, src => src.GraphId)
            .Map(dest => dest.CreatedAtUtc, src => src.CreatedAtUtc);

        // Edge → EdgeDto
        config.NewConfig<Edge, EdgeDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.FromNodeId, src => src.FromNodeId)
            .Map(dest => dest.ToNodeId, src => src.ToNodeId)
            .Map(dest => dest.CreatedAtUtc, src => src.CreatedAtUtc);

        // Document → DocumentDto
        config.NewConfig<Document, DocumentDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.BodyMarkdown, src => src.BodyMarkdown)
            .Map(dest => dest.IsInternal, src => src.IsInternal)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
            .Map(dest => dest.ProjectId, src => src.ProjectId)
            .Map(dest => dest.CreatedAtUtc, src => src.CreatedAtUtc);

        // TaskItem → TaskItemDto
        config.NewConfig<TaskItem, TaskItemDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.ProjectId, src => src.ProjectId)
            .Map(dest => dest.ParentId, src => src.ParentId)
            .Map(dest => dest.CreatedAtUtc, src => src.CreatedAtUtc);

        config.NewConfig(typeof(PaginatedResult<>), typeof(PaginatedResult<>))
            .Map("Items", "Items")
            .Map("TotalCount", "TotalCount")
            .Map("Page", "Page")
            .Map("PageSize", "PageSize");
    }
}