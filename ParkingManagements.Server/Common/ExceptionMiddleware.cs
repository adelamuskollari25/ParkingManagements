using ParkingManagements.Server.Common;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
        catch (ServiceException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;

            var response = new ErrorResponse
            {
                Code = ex.Code,
                Message = ex.Message,
                CorrelationId = context.TraceIdentifier
            };

            _logger.LogError(ex, "Service exception occurred");

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var response = new ErrorResponse
            {
                Code = "server_error",
                Message = ex.Message,
                CorrelationId = context.TraceIdentifier
            };

            _logger.LogError(ex, "Unhandled exception occurred");

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
