using CampusRouteLab.Models;
using CampusRouteLab.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CampusRouteLab.Endpoints;

public static class BusinessEndpoints
{
    public static void MapBusinessEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", () =>
        {
            return TypedResults.Ok(new
            {
                Service = "CampusRouteLab",
                Description = "Diagnostic Web Service",
                Sections = new[] { "/students", "/reports", "portal", "/files", "routes", "/diag/lifetimes" }
            });
        });

        app.MapGet("/students", (IStudentCatalogService catalog) =>
        {
            TypedResults.Ok(catalog.GetGroups());
        });
        
        app.MapGet("/students/{group}", Results<Ok<object>, NotFound> (string group, IStudentCatalogService catalog) =>
        {
            var students = catalog.GetStudentsByGroup(group);
            return students is not null
                ? TypedResults.Ok((object)new { Group = group, Students = students })
                : TypedResults.NotFound();
        });

        app.MapGet("/students/{group}/{id:int}", GetStudentById);
        
        app.MapGet("/reports/{section?}", (string? section) =>
        {
            return TypedResults.Ok(new { Section = section ?? "overview" });
        });

        app.MapGet("/portal/{module=home}/{page=index}/{id?}", (string module, string page, string? id) =>
        {
            return TypedResults.Text($"Module: {module}, page: {page}, id: {id ?? "none"}");
        });

        app.MapGet("/files/{**path}", (string path) =>
        {
            return TypedResults.Text($"Captured path: {path}");
        });
    }

    public static Results<Ok<Student>, NotFound> GetStudentById(string group, int id, IStudentCatalogService catalog)
    {
        var student = catalog.GetStudent(group, id);
        return student is not null ? TypedResults.Ok(student) : TypedResults.NotFound();
    }
}