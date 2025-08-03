using Ardalis.GuardClauses;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Researcher.Api.Common.Middlewares;
using Researcher.Application.Common.Interfaces;
using Researcher.Application.Common.Mappings;
using Researcher.Application.Requests.Projects.Commands;
using Researcher.Domain.ValueObjects;
using Researcher.Infrastructure.DAL;
using Researcher.Infrastructure.DAL.Services;
using Wolverine;

namespace Researcher.Api.Common.Extensions;

/// <summary>
/// Расширения для регистрации сервисов и зависимостей приложения.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует сервисы доступа к данным, включая DbContext, репозитории и UnitOfWork.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <returns>Коллекция сервисов с добавленными зависимостями.</returns>
    public static IServiceCollection AddDataServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Включаем устаревшее поведение обработки временных меток для Npgsql 6+
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Guard.Against.NullOrEmpty(connectionString);

        services.AddDbContext<ResearcherDbContext>(opts =>
            opts.UseNpgsql(
                connectionString,
                npgsql => npgsql.MapEnum<TaskItemStatus>()
            )
        );

        // 2. Репозитории и UnitOfWork
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    /// <summary>
    /// Регистрирует сервисы бизнес-логики и интеграции (Mapster, Wolverine).
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации.</param>
    /// <returns>Коллекция сервисов с добавленными зависимостями.</returns>
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        // 1. Mapster (сканируем текущую сборку на конфиги)
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(MappingProfile).Assembly);
        services.AddSingleton(config);
        services.AddMapster();

        services.AddWolverine(opts =>
        {
            // По умолчанию сканируется entry assembly. Если обработчик в другом проекте:
            opts.Discovery.IncludeAssembly(typeof(CreateProjectCommand).Assembly);
        });

        return services;
    }

    /// <summary>
    /// Регистрирует веб-сервисы, включая middleware, OpenAPI и ProblemDetails.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации.</param>
    /// <returns>Коллекция сервисов с добавленными зависимостями.</returns>
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddEndpointsApiExplorer();
        services.AddSingleton<GlobalExceptionMiddleware>();
        services.AddOpenApi();

        return services;
    }
}