using CampusRouteLab.Services;
using CampusRouteLab.Middleware;
using CampusRouteLab.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCampusServices();
var app = builder.Build();

app.UseRequestAudit();

app.MapBusinessEndpoints();
app.MapDiagnosticEndpoints();

app.Run();