using Ardalis.GuardClauses;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Graphs.Edges.Commands;

/// <summary>
/// Команда удаления ребра.
/// </summary>
/// <param name="EdgeId">Идентификатор ребра.</param>
public record DeleteEdgeCommand(Guid EdgeId);

/// <summary>
/// Обработчик команды удаления ребра.
/// </summary>
public class DeleteEdgeCommandHandler
{
    private readonly IRepository<Edge> _edgeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEdgeCommandHandler(
        IRepository<Edge> edgeRepository,
        IUnitOfWork unitOfWork)
    {
        _edgeRepository = Guard.Against.Null(edgeRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
    }

    /// <summary>
    /// Удаляет ребро по идентификатору.
    /// </summary>
    /// <param name="request">Команда с идентификатором ребра.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача удаления.</returns>
    /// <exception cref="ArgumentException">Если EdgeId — значение по умолчанию.</exception>
    /// <exception cref="InvalidOperationException">Если ребро не найдено.</exception>
    public async Task Handle(
        DeleteEdgeCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.EdgeId);

        var edge = await _edgeRepository.GetByIdAsync(
            request.EdgeId,
            trackChanges: true,
            cancellationToken);

        ThrowIf.EntityIsNull(edge, request.EdgeId);

        await _edgeRepository.RemoveAsync(edge, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}