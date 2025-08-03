using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Documents.Queries;

/// <summary>
/// Запрос деталей документа по идентификатору.
/// </summary>
/// <param name="DocumentId">Идентификатор документа.</param>
public record GetDocumentByIdQuery(Guid DocumentId);

/// <summary>
/// Обработчик запроса получения документа по Id.
/// </summary>
public class GetDocumentByIdQueryHandler
{
    private readonly IRepository<Document> _documentRepository;
    private readonly IMapper _mapper;

    public GetDocumentByIdQueryHandler(
        IRepository<Document> documentRepository,
        IMapper mapper)
    {
        _documentRepository = Guard.Against.Null(documentRepository);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Получает DTO документа по идентификатору.
    /// </summary>
    /// <param name="request">Запрос с Id документа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO документа.</returns>
    /// <exception cref="ArgumentException">Если DocumentId — значение по умолчанию.</exception>
    /// <exception cref="InvalidOperationException">Если документ не найден.</exception>
    public async Task<DocumentDto> Handle(
        GetDocumentByIdQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.DocumentId);

        var doc = await _documentRepository.GetByIdAsync(
            request.DocumentId,
            trackChanges: false,
            cancellationToken);

        ThrowIf.EntityIsNull(doc, request.DocumentId);

        return _mapper.Map<DocumentDto>(doc);
    }
}