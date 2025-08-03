using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;

namespace Researcher.Application.Requests.Documents.Commands;

/// <summary>
/// Команда создания документа.
/// </summary>
/// <param name="ProjectId">Идентификатор проекта.</param>
/// <param name="Title">Заголовок документа.</param>
/// <param name="BodyMarkdown">Содержимое документа в формате Markdown.</param>
/// <param name="IsInternal">Флаг внутреннего документа.</param>
public record CreateDocumentCommand(
    Guid ProjectId,
    string Title,
    string BodyMarkdown,
    bool IsInternal
);

/// <summary>
/// Обработчик команды создания документа.
/// </summary>
public class CreateDocumentCommandHandler
{
    private readonly IRepository<Document> _documentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDocumentCommandHandler(
        IRepository<Document> documentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _documentRepository = Guard.Against.Null(documentRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Создаёт новый документ и возвращает его DTO.
    /// </summary>
    /// <param name="request">Команда с параметрами создания документа.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>DTO созданного документа.</returns>
    /// <exception cref="ArgumentException">Если ProjectId — значение по умолчанию, Title или BodyMarkdown пустые.</exception>
    public async Task<DocumentDto> Handle(
        CreateDocumentCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.ProjectId);
        Guard.Against.NullOrWhiteSpace(request.Title);
        Guard.Against.NullOrWhiteSpace(request.BodyMarkdown);

        var document = new Document(
            Guid.NewGuid(),
            request.Title,
            request.BodyMarkdown,
            request.IsInternal,
            request.ProjectId);

        await _documentRepository.AddAsync(document, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<DocumentDto>(document);
    }
}