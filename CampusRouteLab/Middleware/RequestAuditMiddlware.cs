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

