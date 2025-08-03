using System.Linq.Expressions;
using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Application.Common.Models;
using Researcher.Application.Common.Services;
using Researcher.Domain.Entities;

namespace Researcher.Application.Requests.Documents.Queries;

/// <summary>
/// Запрос списка документов проекта с опциональным фильтром по внутренним/внешним.
/// </summary>
/// <param name="ProjectId">Идентификатор проекта.</param>
/// <param name="IsInternal">Фильтр по типу документа (внутренний/внешний), необязательный.</param>
public record ListDocumentsByProjectQuery(
    Guid ProjectId,
    bool? IsInternal,
    string? OrderBy = null,
    SortDirection SortDirection = SortDirection.Ascending,
    int? Page = null,
    int? PageSize = null);

/// <summary>
/// Обработчик запроса списка документов проекта.
/// </summary>
public class ListDocumentsByProjectQueryHandler
{
    private readonly IRepository<Document> _documentRepository;
    private readonly IMapper _mapper;

    public ListDocumentsByProjectQueryHandler(
        IRepository<Document> documentRepository,
        IMapper mapper)
    {
        _documentRepository = Guard.Against.Null(documentRepository);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Возвращает пагинированный список документов с фильтром по признаку внутреннего документа.
    /// </summary>
    /// <param name="request">Запрос с идентификатором проекта и опциональным фильтром.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Пагинированный результат DTO документов.</returns>
    /// <exception cref="ArgumentException">Если ProjectId — значение по умолчанию.</exception>
    public async Task<PaginatedResult<DocumentDto>> Handle(
        ListDocumentsByProjectQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.ProjectId);
        Guard.Against.NegativeOrZero(request.Page ?? 1);
        Guard.Against.NegativeOrZero(request.PageSize ?? 1);
        if (request.OrderBy is not null)
            Guard.Against.NullOrWhiteSpace(request.OrderBy);

        Expression<Func<Document, object>>? orderByExpr = null;
        var ascending = true;

        if (!string.IsNullOrWhiteSpace(request.OrderBy))
        {
            orderByExpr = ReflectionHelper.GetPropertyExpression<Document>(request.OrderBy!);
            ascending = request.SortDirection == SortDirection.Ascending;
        }

        var result = await _documentRepository.QueryAsync(
            predicate: d => d.ProjectId == request.ProjectId
                            && (request.IsInternal == null || d.IsInternal == request.IsInternal.Value),
            orderBy: orderByExpr,
            ascending: ascending,
            page: request.Page,
            pageSize: request.PageSize,
            trackChanges: false,
            cancellationToken: cancellationToken);

        return _mapper.Map<PaginatedResult<DocumentDto>>(result);
    }
}