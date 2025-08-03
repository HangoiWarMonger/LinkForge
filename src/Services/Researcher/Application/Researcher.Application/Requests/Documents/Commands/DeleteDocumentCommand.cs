using Ardalis.GuardClauses;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Documents.Commands;

/// <summary>
/// Команда удаления документа.
/// </summary>
/// <param name="DocumentId">Идентификатор документа.</param>
public record DeleteDocumentCommand(Guid DocumentId);

/// <summary>
/// Обработчик команды удаления документа.
/// </summary>
public class DeleteDocumentCommandHandler
{
    private readonly IRepository<Document> _documentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDocumentCommandHandler(
        IRepository<Document> documentRepository,
        IUnitOfWork unitOfWork)
    {
        _documentRepository = Guard.Against.Null(documentRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
    }

    /// <summary>
    /// Удаляет документ по идентификатору.
    /// </summary>
    /// <param name="request">Команда с идентификатором документа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача удаления.</returns>
    /// <exception cref="ArgumentException">Если DocumentId — значение по умолчанию.</exception>
    /// <exception cref="InvalidOperationException">Если документ не найден.</exception>
    public async Task Handle(
        DeleteDocumentCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.DocumentId);

        var document = await _documentRepository.GetByIdAsync(
            request.DocumentId,
            trackChanges: true,
            cancellationToken);

        ThrowIf.EntityIsNull(document, request.DocumentId);

        await _documentRepository.RemoveAsync(document, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}