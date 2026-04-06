using CampusRouteLab.Models;

namespace CampusRouteLab.Services;

public interface IStudentCatalogService
{
    IEnumerable<GroupInfo> GetGroups();
    IEnumerable<Student>? GetStudentsByGroup(string groupName);
    Student? GetStudent(string groupName, int studentId);
}

public interface IAppInfoService
{
    Guid AppInstanceId { get; }
    DateTime StartedAt { get; }
}

public interface IRequestContextService
{
    Guid RequestId { get; }
    DateTime CreatedAt { get; }
}

public interface ITransientMarkerService
{
    Guid MarkerId { get; }
}