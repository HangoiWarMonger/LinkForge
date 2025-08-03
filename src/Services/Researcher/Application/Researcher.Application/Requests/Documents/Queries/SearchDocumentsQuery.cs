using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Application.Common.Models;
using Researcher.Domain.Entities;

namespace Researcher.Application.Requests.Documents.Queries;

/// <summary>
/// Поиск документов по тексту и опциональному проекту.
/// </summary>
/// <param name="ProjectId">Идентификатор проекта (необязательный).</param>
/// <param name="SearchText">Текст для поиска.</param>
public record SearchDocumentsQuery(Guid? ProjectId, string SearchText);

/// <summary>
/// Обработчик запроса поиска документов.
/// </summary>
public class SearchDocumentsQueryHandler
{
    private readonly IRepository<Document> _documentRepository;
    private readonly IMapper _mapper;

    public SearchDocumentsQueryHandler(
        IRepository<Document> documentRepository,
        IMapper mapper)
    {
        _documentRepository = Guard.Against.Null(documentRepository);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Выполняет поиск документов по тексту с возможным фильтром по проекту.
    /// </summary>
    /// <param name="request">Запрос с параметрами поиска.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Пагинированный результат DTO документов.</returns>
    /// <exception cref="ArgumentException">Если SearchText пустой или null.</exception>
    public async Task<PaginatedResult<DocumentDto>> Handle(
        SearchDocumentsQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.NullOrWhiteSpace(request.SearchText);

        var docs = await _documentRepository.QueryAsync(
            d => (request.ProjectId == null || d.ProjectId == request.ProjectId)
                 && (d.Title.Contains(request.SearchText)
                     || d.BodyMarkdown.Contains(request.SearchText)),
            trackChanges: false,
            cancellationToken: cancellationToken);

        return _mapper.Map<PaginatedResult<DocumentDto>>(docs);
    }
}