using Scalar.AspNetCore;

namespace Researcher.Api.Common.Extensions.EndpointsExtensions;

/// <summary>
/// Расширения для регистрации групп эндпоинтов API.
/// </summary>
public static class ApiEndpoints
{
    /// <summary>
    /// Регистрирует основные группы эндпоинтов приложения под префиксом /api.
    /// </summary>
    /// <param name="app">Роутер для регистрации эндпоинтов.</param>
    public static void MapApiEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api");

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(opt =>
            {
                opt.Title = "Researcher.Api";
                opt.Theme = ScalarTheme.DeepSpace;
                opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
            });
        }

        group.MapDocumentEndpoints();
        group.MapProjectEndpoints();
        group.MapGraphEndpoints();
        group.MapTaskEndpoints();
    }
}