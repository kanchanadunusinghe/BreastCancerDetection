using BCD.Application.DTOs.Common;
using BCD.Application.DTOs.MachineLearningDTOs;
using Microsoft.AspNetCore.Http;

namespace BCD.Application.Common.Interfaces;

public interface IBCDMachineLearningService
{
    Task<ResultDto<BreastCancerPredictionResponseDto>> PredictBreastCancerAsync(
        IFormFile file, CancellationToken cancellationToken = default);
}
