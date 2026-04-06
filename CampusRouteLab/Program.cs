using CampusRouteLab.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRequestAudit();

app.Run();