using Ardalis.GuardClauses;
using MapsterMapper;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Interfaces;
using Researcher.Domain.Entities;
using Researcher.Domain.Validation;

namespace Researcher.Application.Requests.Graphs.Edges.Queries;

/// <summary>
/// Запрос деталей ребра по идентификатору.
/// </summary>
/// <param name="EdgeId">Идентификатор ребра.</param>
public record GetEdgeByIdQuery(Guid EdgeId);

/// <summary>
/// Обработчик запроса получения ребра по Id.
/// </summary>
public class GetEdgeByIdQueryHandler
{
    private readonly IRepository<Edge> _edgeRepository;
    private readonly IMapper _mapper;

    public GetEdgeByIdQueryHandler(
        IRepository<Edge> edgeRepository,
        IMapper mapper)
    {
        _edgeRepository = Guard.Against.Null(edgeRepository);
        _mapper = Guard.Against.Null(mapper);
    }

    /// <summary>
    /// Получает DTO ребра по идентификатору.
    /// </summary>
    /// <param name="request">Запрос с Id ребра.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>DTO ребра.</returns>
    /// <exception cref="ArgumentException">Если EdgeId — значение по умолчанию.</exception>
    /// <exception cref="InvalidOperationException">Если ребро не найдено.</exception>
    public async Task<EdgeDto> Handle(
        GetEdgeByIdQuery request,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        Guard.Against.Default(request.EdgeId);

        var edge = await _edgeRepository.GetByIdAsync(
            request.EdgeId,
            trackChanges: false,
            cancellationToken);

        ThrowIf.EntityIsNull(edge, request.EdgeId);

        return _mapper.Map<EdgeDto>(edge);
    }
}