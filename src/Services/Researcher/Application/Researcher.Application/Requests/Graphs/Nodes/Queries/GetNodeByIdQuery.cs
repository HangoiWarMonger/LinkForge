using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Graphs.Nodes.Queries;

/// <summary>
/// Запрос деталей узла по идентификатору, включая связанные ребра.
/// </summary>
/// <param name="NodeId">Идентификатор узла.</param>
public record GetNodeByIdQuery(Guid NodeId);

/// <summary>
/// Обработчик запроса получения узла по Id.
/// </summary>
public class GetNodeByIdQueryHandler
{
    private readonly IRepository<Node> _nodeRepository;
    private readonly IMapper _mapper;

    public GetNodeByIdQueryHandler(
        IRepository<Node> nodeRepository,
        IMapper mapper)
    {
        _nodeRepository = Guard.Against.Null(nodeRepository);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Получает DTO узла по идентификатору.
    /// </summary>
    /// <param name="request">Запрос с Id узла.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO узла.</returns>
    /// <exception cref="ArgumentException">Если NodeId — значение по умолчанию.</exception>
    /// <exception cref="InvalidOperationException">Если узел не найден.</exception>
    public async Task<NodeDto> Handle(
        GetNodeByIdQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.NodeId);

        var node = await _nodeRepository.GetByIdAsync(
            request.NodeId,
            trackChanges: false,
            cancellationToken);

        ThrowIf.EntityIsNull(node, request.NodeId);

        return _mapper.Map<NodeDto>(node);
    }
}