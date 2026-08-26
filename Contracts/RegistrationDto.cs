using RegSystemAPI.Models;

namespace RegSystemAPI.Contracts;

public sealed class RegistrationDto
{
    public int RegId { get; init; }
    public string StudentId { get; init; } = string.Empty;
    public int SectionId { get; init; }
    public string Semester { get; init; } = string.Empty;
    public string RegStatus { get; init; } = string.Empty;
    public DateTime RegisteredAt { get; init; }
    public SectionDto? Section { get; init; }

    public static RegistrationDto FromRegistration(Registration registration) => new()
    {
        RegId = registration.RegId,
        StudentId = registration.StudentId,
        SectionId = registration.SectionId,
        Semester = registration.Semester,
        RegStatus = registration.RegStatus,
        RegisteredAt = registration.RegisteredAt,
        Section = registration.Section is null ? null : SectionDto.FromSection(registration.Section)
    };
}

public sealed class SectionDto
{
    public int SectionId { get; init; }
    public string CourseCode { get; init; } = string.Empty;
    public string SecNo { get; init; } = string.Empty;
    public string ScheduleTime { get; init; } = string.Empty;
    public string Instructor { get; init; } = string.Empty;
    public int Capacity { get; init; }
    public int Enrolled { get; init; }
    public CourseDto? Course { get; init; }

    public static SectionDto FromSection(Section section) => new()
    {
        SectionId = section.SectionId,
        CourseCode = section.CourseCode,
        SecNo = section.SecNo,
        ScheduleTime = section.ScheduleTime,
        Instructor = section.Instructor,
        Capacity = section.Capacity,
        Enrolled = section.Enrolled,
        Course = section.Course is null ? null : new CourseDto
        {
            CourseCode = section.Course.CourseCode,
            CourseName = section.Course.CourseName,
            Credits = section.Course.Credits,
            Category = section.Course.Category,
            PrerequisiteCode = section.Course.PrerequisiteCode
        }
    };
}

public sealed class CourseDto
{
    public string CourseCode { get; init; } = string.Empty;
    public string CourseName { get; init; } = string.Empty;
    public int Credits { get; init; }
    public string Category { get; init; } = string.Empty;
    public string? PrerequisiteCode { get; init; }
}
