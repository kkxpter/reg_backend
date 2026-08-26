using RegSystemAPI.Models;

namespace RegSystemAPI.Contracts;

public sealed class StudentProfileDto
{
    public string StudentId { get; init; } = string.Empty;
    public string NationalId { get; init; } = string.Empty;
    public string FirstNameTh { get; init; } = string.Empty;
    public string LastNameTh { get; init; } = string.Empty;
    public string UniversityEmail { get; init; } = string.Empty;
    public string? PersonalEmail { get; init; }
    public string? Phone { get; init; }
    public string? Province { get; init; }
    public string? Faculty { get; init; }
    public string? Major { get; init; }
    public string? Advisor { get; init; }
    public decimal Gpax { get; init; }
    public int TotalCredits { get; init; }
    public string Status { get; init; } = string.Empty;

    public static StudentProfileDto FromStudent(Student student) => new()
    {
        StudentId = student.StudentId,
        NationalId = student.NationalId,
        FirstNameTh = student.FirstNameTh,
        LastNameTh = student.LastNameTh,
        UniversityEmail = student.UniversityEmail,
        PersonalEmail = student.PersonalEmail,
        Phone = student.Phone,
        Province = student.Province,
        Faculty = student.Faculty,
        Major = student.Major,
        Advisor = student.Advisor,
        Gpax = student.Gpax,
        TotalCredits = student.TotalCredits,
        Status = student.Status
    };
}

public sealed class UpdateStudentProfileDto
{
    public string? PersonalEmail { get; init; }
    public string? Phone { get; init; }
    public string? Province { get; init; }
}
