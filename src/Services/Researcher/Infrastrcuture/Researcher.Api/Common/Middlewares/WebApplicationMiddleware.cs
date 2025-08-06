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
            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        return app;
    }
}