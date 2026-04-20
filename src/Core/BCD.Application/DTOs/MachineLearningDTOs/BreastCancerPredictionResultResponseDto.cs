using BCD.Application.DTOs.PatientDTOs;

namespace BCD.Application.DTOs.MachineLearningDTOs
{
    public class BreastCancerPredictionResultResponseDto
    {
        public PatientRequestDto Patient { get; set; }
        public int RecordId { get; set; }
        public string? Comment { get; set; }
        public string CancerType { get; set; } = string.Empty;
        public decimal Probability { get; set; }
        public string? ExrayImageUrl { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
        public string CreatedUserName { get; set; } = string.Empty;

    }
}
