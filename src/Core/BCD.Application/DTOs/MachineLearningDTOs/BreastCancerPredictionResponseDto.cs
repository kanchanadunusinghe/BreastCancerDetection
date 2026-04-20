namespace BCD.Application.DTOs.MachineLearningDTOs;

public class BreastCancerPredictionResponseDto
{
    public string CancerType { get; set; } = string.Empty;
    public decimal Probability { get; set; }
}
