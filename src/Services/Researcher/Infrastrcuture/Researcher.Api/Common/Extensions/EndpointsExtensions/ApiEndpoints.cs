namespace Researcher.Api.Common.Extensions.EndpointsExtensions;

/// <summary>
/// Расширения для регистрации групп эндпоинтов API.
/// </summary>
public static class ApiEndpoints
{
    /// <summary>
    /// Регистрирует основные группы эндпоинтов приложения под префиксом /api.
    /// </summary>
    /// <param name="routes">Роутер для регистрации эндпоинтов.</param>
    public static void MapApiEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api");

        group.MapDocumentEndpoints();
        group.MapProjectEndpoints();
        group.MapGraphEndpoints();
        group.MapTaskEndpoints();
    }
}