using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Documents.Commands;

/// <summary>
/// Команда обновления документа.
/// </summary>
/// <param name="DocumentId">Идентификатор документа.</param>
/// <param name="NewTitle">Новое название документа.</param>
/// <param name="NewBodyMarkdown">Новое содержимое документа в формате Markdown.</param>
/// <param name="NewIsInternal">Новый флаг внутреннего документа.</param>
public record UpdateDocumentCommand(
    Guid DocumentId,
    string NewTitle,
    string NewBodyMarkdown,
    bool NewIsInternal
);

/// <summary>
/// Обработчик команды обновления документа.
/// </summary>
public class UpdateDocumentCommandHandler
{
    private readonly IRepository<Document> _documentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDocumentCommandHandler(
        IRepository<Document> documentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _documentRepository = Guard.Against.Null(documentRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Обновляет документ и возвращает актуальный DTO.
    /// </summary>
    /// <param name="request">Команда с новыми параметрами документа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO обновленного документа.</returns>
    /// <exception cref="ArgumentException">Если DocumentId — значение по умолчанию, NewTitle или NewBodyMarkdown пусты.</exception>
    /// <exception cref="InvalidOperationException">Если документ не найден.</exception>
    public async Task<DocumentDto> Handle(
        UpdateDocumentCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.DocumentId);
        Guard.Against.NullOrWhiteSpace(request.NewTitle);
        Guard.Against.NullOrWhiteSpace(request.NewBodyMarkdown);

        var document = await _documentRepository.GetByIdAsync(
            request.DocumentId,
            trackChanges: true,
            cancellationToken);

        ThrowIf.EntityIsNull(document, request.DocumentId);

        document.Update(
            request.NewTitle,
            request.NewBodyMarkdown,
            request.NewIsInternal);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<DocumentDto>(document);
    }
}