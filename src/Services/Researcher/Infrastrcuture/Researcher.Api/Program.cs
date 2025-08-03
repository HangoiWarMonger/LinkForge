using Researcher.Api.Common.Extensions;
using Researcher.Api.Common.Extensions.EndpointsExtensions;
using Researcher.Api.Common.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddDataServices(builder.Configuration)
    .AddWebServices();

var app = builder.Build();

app.UseWebServices();
app.MapApiEndpoints();

await app.RunAsync();