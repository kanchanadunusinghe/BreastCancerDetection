namespace BCD.Application.DTOs.PatientDTOs
{
    public class MammographyScanResultDto
    {
        public int Id { get; set; }
        public string RecordId { get; set; } = string.Empty;
        public string? Comment { get; set; }
        public string CancerType { get; set; } = string.Empty;
        public decimal Probability { get; set; }
        public string? ExrayImageUrl { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public string CreatedUserName { get; set; } = string.Empty;

        public PatientRequestDto Patient { get; set; }
    }
}
