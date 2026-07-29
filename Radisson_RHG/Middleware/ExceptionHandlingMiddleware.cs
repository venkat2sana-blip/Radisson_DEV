using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Radisson_RHG.Exceptions;

namespace Radisson_RHG.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception caught by middleware");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext ctx, Exception ex)
    {
        ctx.Response.ContentType = "application/json";

        var status = ex switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            AppException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var payload = ex switch
        {
            NotFoundException nf => new { title = nf.Message },
            AppException ae => new { title = ae.Message },
            _ => new { title = "An unexpected error occurred." }
        };

        ctx.Response.StatusCode = status;
        var json = JsonSerializer.Serialize(payload);
        return ctx.Response.WriteAsync(json);
    }
}
