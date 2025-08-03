using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Ardalis.GuardClauses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Researcher.Api.Common.Models;
using Researcher.Domain.Exceptions;

namespace Researcher.Api.Common.Middlewares;

/// <summary>
/// Обрабатывает все необработанные исключения в конвейере ASP.NET Core
/// и возвращает стандартизированный ответ в формате ProblemDetails или ErrorResponse.
/// </summary>
public class GlobalExceptionMiddleware : IMiddleware
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private readonly IProblemDetailsService _problemDetailsService;

    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    /// <summary>
    /// Создаёт экземпляр middleware для глобальной обработки исключений.
    /// </summary>
    /// <param name="problemDetailsService">Сервис для записи ProblemDetails в ответ.</param>
    /// <param name="logger">Логгер для записи деталей ошибок.</param>
    public GlobalExceptionMiddleware(
        IProblemDetailsService problemDetailsService,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        Guard.Against.Null(problemDetailsService);
        Guard.Against.Null(logger);

        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception processing request {Path}", context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;

        switch (exception)
        {
            case ValidationException validationException:
                await WriteProblemDetailsAsync(context, validationException, HttpStatusCode.BadRequest);
                return;

            case EntityNotFoundException notFound:
                await WriteProblemDetailsAsync(context, notFound, HttpStatusCode.NotFound);
                return;

            case ArgumentException argEx:
                await WriteProblemDetailsAsync(context, argEx, HttpStatusCode.BadRequest);
                return;

            default:
                // Не-ProblemDetails ответ
                await WriteErrorResponseAsync(context, exception);
                return;
        }
    }

    private async Task WriteProblemDetailsAsync(
        HttpContext context,
        Exception exception,
        HttpStatusCode httpStatus)
    {
        var statusCode = (int) httpStatus;
        context.Response.StatusCode = statusCode;

        ProblemDetails problemDetails;

        if (exception is ValidationException validationException)
        {
            var modelState = new ModelStateDictionary();
            foreach (var failure in validationException.Errors)
                modelState.AddModelError(failure.PropertyName, failure.ErrorMessage);

            problemDetails = new ValidationProblemDetails(modelState);
        }
        else
        {
            problemDetails = new ProblemDetails
            {
                Detail = exception.Message
            };
        }

        problemDetails.Status = statusCode;
        problemDetails.Instance = context.Request.Path;

        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = context,
            ProblemDetails = problemDetails
        };
        await _problemDetailsService.WriteAsync(problemDetailsContext);
    }

    private static async Task WriteErrorResponseAsync(HttpContext context, Exception exception)
    {
        // 500 InternalServerError уже установлен в статусе по умолчанию
        var errorResponse = new ErrorResponse
        {
            Type = exception.GetType().FullName ?? "Exception",
            Message = exception.Message,
            StackTrace = exception.StackTrace,
            Data = exception.Data
        };

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, JsonSerializerOptions));
    }
}