using CampusRouteLab.Models;

namespace CampusRouteLab.Services;

public class StudentCatalogService : IStudentCatalogService
{
    private readonly Dictionary<string, List<Student>> _data = new(StringComparer.OrdinalIgnoreCase)
    {
        {"CS-101", [new(1, "Alice"), new(2, "Bob")]},
        {"IT-201", [new(3, "Charlie"), new(4, "Diana"), new(5, "Eve")]}
    };
    
    public IEnumerable<GroupInfo> GetGroups() => _data.Select(kvp => new GroupInfo(kvp.Key, kvp.Value.Count));
    
    public IEnumerable<Student>? GetStudentsByGroup(string groupName) =>
        _data.TryGetValue(groupName, out List<Student> students) ? students : null;

    public Student? GetStudent(string groupName, int studentId) =>
        GetStudentsByGroup(groupName)?.FirstOrDefault(s => s.Id == studentId);
}

public class AppInfoService : IAppInfoService
{
    public Guid AppInstanceId { get; } = Guid.NewGuid();
    public DateTime StartedAt { get; } = DateTime.UtcNow;
}

public class RequestContextService : IRequestContextService
{
    public Guid RequestId { get; } = Guid.NewGuid();
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}

public class TransientMarkerService : ITransientMarkerService
{
    public Guid MarkerId { get; } = Guid.NewGuid();
}

public class DiagnosticReportService(
    IAppInfoService appInfo,
    IRequestContextService requestContext,
    ITransientMarkerService transientMarker)
{
    public object GenerateReport() => new
    {
        AppInfo = new { appInfo.AppInstanceId, appInfo.StartedAt },
        Request = new { requestContext.RequestId, requestContext.CreatedAt },
        Transient = new { transientMarker.MarkerId }
    };
}