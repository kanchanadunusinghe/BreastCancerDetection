using Microsoft.AspNetCore.Http;

namespace BCD.Application.DTOs.MachineLearningDTOs;

public class BreastCancerPredictionRequestDto
{
    public IFormFile ExrayImage { get; set; } = default!;
    public int PatientId { get; set; }
    public int LoggedInUserId { get; set; }
}
