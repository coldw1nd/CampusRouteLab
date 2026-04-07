using CampusRouteLab.Services;

namespace CampusRouteLab.Endpoints;

public static class DiagnosticEndpoints
{
    public static void MapDiagnosticEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/routes", (EndpointDataSource dataSource) =>
        {
            var routes = dataSource.Endpoints.OfType<RouteEndpoint>().Select(e => e.RoutePattern.RawText);
            return string.Join(Environment.NewLine, routes);
        });

        app.MapGet("/diag/lifetimes", (
            IAppInfoService appInfo,
            IRequestContextService requestContext,
            ITransientMarkerService transientMarker,
            DiagnosticReportService diagnosticReportService) => TypedResults.Ok(new
        {
            Direct = new { appInfo.AppInstanceId, requestContext.RequestId, TransientId = transientMarker.MarkerId },
            FromService = diagnosticReportService.GenerateReport()
        }));

        app.MapGet("/diag/lifetimes/check", (
            ITransientMarkerService transientMarker1,
            ITransientMarkerService transientMarker2) =>
        {
            return TypedResults.Ok(new
            {
                Transient1 = transientMarker1.MarkerId,
                Transient2 = transientMarker2.MarkerId,
                AreSame = transientMarker1.MarkerId == transientMarker2.MarkerId
            });
        });

        app.MapGet("/diag/request-services", (HttpContext context) =>
        {
            var requestContextService = context.RequestServices.GetRequiredService<IRequestContextService>();
            return TypedResults.Ok(new { requestContextService.RequestId, requestContextService.CreatedAt });
        });

        app.MapGet("/diag/app-services", () =>
        {
            var appInfoService = app.ServiceProvider.GetRequiredService<IAppInfoService>();
            return TypedResults.Ok(new
            {
                Source = "Global App Service",
                appInfoService.AppInstanceId,
                appInfoService.StartedAt
            });
        });
    }
}