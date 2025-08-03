namespace Researcher.Domain.Exceptions;

/// <summary>
/// Выбрасывается, когда запрашиваемая сущность не найдена.
/// </summary>
public class EntityNotFoundException : Exception
{
    /// <param name="entityName">Имя типа сущности.</param>
    /// <param name="id">Идентификатор сущности.</param>
    public EntityNotFoundException(string entityName, Guid id)
        : base($"Entity '{entityName}' (Id = {id}) not found.")
    {
    }
}