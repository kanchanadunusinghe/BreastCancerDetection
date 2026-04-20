using BCD.Domain.Enums;

namespace BCD.Application.DTOs.PatientDTOs;

public class PatientListDto
{
    public int Id { get; set; }

    public string NHSNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}".Trim();

    public string Email { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public int Age =>
        DateTime.Today.Year - DateOfBirth.Year -
        (DateTime.Today.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);

    public string Gender { get; set; } = string.Empty;
    public Gender Sex { get; set; }

    public string PostCode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public int MammographyScanCount { get; set; }
    public string Status { get; set; } = string.Empty;
}
