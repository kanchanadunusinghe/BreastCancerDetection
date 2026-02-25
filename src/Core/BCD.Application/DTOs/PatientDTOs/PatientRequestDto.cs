using BCD.Domain.Enums;

namespace BCD.Application.DTOs.PatientDTOs
{
    public class PatientRequestDto
    {
        public int Id { get; set; }
        public string NHSNumber { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string PostCode { get; set; } = string.Empty;
    }
}
