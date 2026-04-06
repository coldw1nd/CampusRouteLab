using CampusRouteLab.Middleware;

namespace CampusRouteLab.Services;

public static class RequestAuditExtensions
{
    public static IApplicationBuilder UseRequestAudit(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestAuditMiddleware>();
    }
}