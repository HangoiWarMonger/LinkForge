using Ardalis.GuardClauses;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Graphs.Commands;

/// <summary>
/// Команда удаления графа.
/// </summary>
/// <param name="GraphId">Идентификатор графа.</param>
public record DeleteGraphCommand(Guid GraphId);

/// <summary>
/// Обработчик команды удаления графа.
/// </summary>
public class DeleteGraphCommandHandler
{
    private readonly IRepository<Graph> _graphRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGraphCommandHandler(
        IRepository<Graph> graphRepository,
        IUnitOfWork unitOfWork)
    {
        _graphRepository = Guard.Against.Null(graphRepository);
        _unitOfWork = Guard.Against.Null(unitOfWork);
    }

    /// <summary>
    /// Удаляет граф по идентификатору.
    /// </summary>
    /// <param name="request">Команда с идентификатором графа.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Задача удаления.</returns>
    /// <exception cref="ArgumentException">Если GraphId — значение по умолчанию.</exception>
    /// <exception cref="InvalidOperationException">Если граф не найден.</exception>
    public async Task Handle(
        DeleteGraphCommand request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.GraphId);

        var graph = await _graphRepository.GetByIdAsync(
            request.GraphId,
            trackChanges: true,
            cancellationToken);

        ThrowIf.EntityIsNull(graph, request.GraphId);

        await _graphRepository.RemoveAsync(graph, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
