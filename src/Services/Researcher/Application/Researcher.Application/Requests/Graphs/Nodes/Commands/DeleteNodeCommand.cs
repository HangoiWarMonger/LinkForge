using Ardalis.GuardClauses;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Graphs.Nodes.Commands;

/// <summary>
/// Команда удаления узла.
/// </summary>
/// <param name="NodeId">Идентификатор узла.</param>
public record DeleteNodeCommand(Guid NodeId);

/// <summary>
/// Обработчик команды удаления узла.
/// </summary>
public class DeleteNodeCommandHandler
{
    private readonly IRepository<Node> _nodeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteNodeCommandHandler(
        IRepository<Node> nodeRepository,
        IUnitOfWork unitOfWork)
    {
        _nodeRepository = Guard.Against.Null(nodeRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
    }

    /// <summary>
    /// Удаляет узел по идентификатору.
    /// </summary>
    /// <param name="request">Команда с идентификатором узла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача удаления.</returns>
    /// <exception cref="ArgumentException">Если NodeId — значение по умолчанию.</exception>
    /// <exception cref="InvalidOperationException">Если узел не найден.</exception>
    public async Task Handle(
        DeleteNodeCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.NodeId);

        var node = await _nodeRepository.GetByIdAsync(
            request.NodeId,
            trackChanges: true,
            cancellationToken);

        ThrowIf.EntityIsNull(node, request.NodeId);

        await _nodeRepository.RemoveAsync(node, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}