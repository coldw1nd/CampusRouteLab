using CampusRouteLab.Services;

namespace CampusRouteLab.Middleware;

public class RequestAuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAppInfoService _appInfo;
    
    public RequestAuditMiddleware(RequestDelegate next, IAppInfoService appInfo)
    {
        _next = next;
        _appInfo = appInfo;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IRequestContextService requestContext,
        ITransientMarkerService transientMarker)
    {
        Console.WriteLine($"[{DateTime.UtcNow:O}] Request started: {context.Request.Path}");

        if (context.Request.Path.StartsWithSegments("/diag"))
        {
            Console.WriteLine("[DI Info]: Transient and Scoped services resolved via InvokeAsync parameters");
            Console.WriteLine("[DI Info]: Singleton service resolved via Middleware constructor");
        }
        
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Append("X-App-Instance", _appInfo.AppInstanceId.ToString());
            context.Response.Headers.Append("X-Request-Id", requestContext.RequestId.ToString());
            context.Response.Headers.Append("X-Transient-Id", transientMarker.MarkerId.ToString());
            
            return Task.CompletedTask;
        });

        await _next(context);
    }
}

