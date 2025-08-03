using Microsoft.EntityFrameworkCore;
using Researcher.Api.Common.Extensions;
using Researcher.Api.Common.Extensions.EndpointsExtensions;
using Researcher.Api.Common.Middlewares;
using Researcher.Infrastructure.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddDataServices(builder.Configuration)
    .AddWebServices();

var app = builder.Build();

// Временный мигратор базы данных
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ResearcherDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseWebServices();
app.MapApiEndpoints();

await app.RunAsync();