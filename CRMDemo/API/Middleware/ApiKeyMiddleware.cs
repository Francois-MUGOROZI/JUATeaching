namespace API.Middleware;

public class ApiKeyMiddleware
{
    private const string ApiKeyHeader = "X-Api-Key";
    private const string ApiKeyConfigKey = "ApiSettings:ApiKey";

    private readonly RequestDelegate _next;
    private readonly string _apiKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _apiKey = configuration[ApiKeyConfigKey]
            ?? throw new InvalidOperationException(
                $"API key not configured. Set '{ApiKeyConfigKey}' in appsettings.");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var providedKey) ||
            !string.Equals(providedKey, _apiKey, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(
                new { error = "Unauthorized. A valid API key is required." });
            return;
        }

        await _next(context);
    }
}
