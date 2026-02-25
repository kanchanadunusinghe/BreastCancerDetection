using BCD.Domain.Enums;

namespace BCD.Application.DTOs.PatientDTOs
{
    public class PatientFilterDto
    {
        public string? Search { get; set; }
        public Gender Gender { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
