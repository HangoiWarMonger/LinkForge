using Scalar.AspNetCore;

namespace Researcher.Api.Common.Middlewares;

/// <summary>
/// Расширения для подключения middleware и сервисов в конвейере ASP.NET Core.
/// </summary>
public static class WebApplicationMiddleware
{
    /// <summary>
    /// Регистрирует middleware и конфигурирует веб-сервисы для приложения.
    /// </summary>
    /// <param name="app">Экземпляр веб-приложения.</param>
    /// <returns>Тот же экземпляр приложения для цепочки вызовов.</returns>
    public static WebApplication UseWebServices(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();

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

        app.UseHttpsRedirection();

        app.UseRouting();

        return app;
    }
}