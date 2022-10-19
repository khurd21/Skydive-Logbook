using System.Text.Json;
using LogbookService.Exceptions;

namespace Logbook.Authorization;

public class ErrorHandlerMiddleware
{
    private RequestDelegate Next { get; init; }

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        this.Next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogger logger)
    {
        try
        {
            await this.Next(context);
        }
        catch (Exception ex)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";

            switch (ex)
            {
                case SkydiverAlreadyExistsException:
                case JumpAlreadyExistsException:
                    response.StatusCode = StatusCodes.Status409Conflict;
                    break;
                case SkydiverNotFoundException:
                case JumpNotFoundException:
                case USPANumberTakenException:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    break;
                default:
                    logger.LogError(ex, "Unhandled exception");
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            await response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
        }
    }
}