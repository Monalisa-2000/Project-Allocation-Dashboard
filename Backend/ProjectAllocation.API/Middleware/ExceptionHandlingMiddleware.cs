using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectAllocation.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (status, title) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
            UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "Forbidden"),
            InvalidOperationException => (StatusCodes.Status409Conflict, "Conflict"),
            _ => (StatusCodes.Status500InternalServerError, "Internal server error")
        };

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";

        var payload = new ProblemDetailsEnvelope(problem);
        return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }

    private sealed class ProblemDetailsEnvelope
    {
        public ProblemDetailsEnvelope(ProblemDetails problem)
        {
            Type = problem.Type;
            Title = problem.Title;
            Status = problem.Status;
            Detail = problem.Detail;
            Instance = problem.Instance;
            Message = problem.Detail ?? problem.Title ?? "An error occurred.";
        }

        [JsonPropertyName("type")]
        public string? Type { get; }

        [JsonPropertyName("title")]
        public string? Title { get; }

        [JsonPropertyName("status")]
        public int? Status { get; }

        [JsonPropertyName("detail")]
        public string? Detail { get; }

        [JsonPropertyName("instance")]
        public string? Instance { get; }

        [JsonPropertyName("message")]
        public string Message { get; }
    }
}
