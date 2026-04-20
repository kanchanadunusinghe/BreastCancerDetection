using BCD.Application.Common.Constants;
using BCD.Application.Common.Interfaces;
using BCD.Application.DTOs.Common;
using BCD.Application.DTOs.MachineLearningDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace BCD.Infrastructure.Services;

public class BCDMachineLearningService(
 HttpClient httpClient,
 IBCDContext dbContext,
 ILogger<BCDMachineLearningService> logger)
 : IBCDMachineLearningService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IBCDContext _dbContext = dbContext;
    private readonly ILogger<BCDMachineLearningService> _logger = logger;

    public async Task<ResultDto<BreastCancerPredictionResponseDto>> PredictBreastCancerAsync(
        IFormFile file, CancellationToken cancellationToken = default)
    {
        if (!IsValidFile(file))
        {
            return ResultDto<BreastCancerPredictionResponseDto>
                .Failure("Invalid or empty file.");
        }

        try
        {
            var apiUrl = await GetFlaskApiUrlAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(apiUrl))
            {
                _logger.LogError("Breast cancer prediction API URL not configured.");
                return ResultDto<BreastCancerPredictionResponseDto>
                    .Failure("Prediction service configuration error.");
            }

            using var content = BuildMultipartContent(file);

            using var response = await _httpClient.PostAsync(
                $"{apiUrl.TrimEnd('/')}/predict",
                content,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);

                _logger.LogError(
                    "Flask API returned error. StatusCode: {StatusCode}, Response: {Response}",
                    response.StatusCode,
                    errorContent);

                return ResultDto<BreastCancerPredictionResponseDto>
                    .Failure("Prediction service failed.");
            }

            var result = await response.Content
                .ReadFromJsonAsync<BreastCancerPredictionResponseDto>(cancellationToken: cancellationToken);

            if (result is null)
            {
                _logger.LogError("Flask API returned null response.");
                return ResultDto<BreastCancerPredictionResponseDto>
                    .Failure("Invalid prediction response.");
            }

            return ResultDto<BreastCancerPredictionResponseDto>.Ok(result);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Breast cancer prediction request was cancelled.");
            return ResultDto<BreastCancerPredictionResponseDto>
                .Failure("Request was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during breast cancer prediction.");
            return ResultDto<BreastCancerPredictionResponseDto>
                .Failure("Unexpected system error.");
        }
    }

    private static bool IsValidFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        var allowedTypes = new[] { "image/jpeg", "image/png" };
        return allowedTypes.Contains(file.ContentType);
    }

    private async Task<string?> GetFlaskApiUrlAsync(CancellationToken cancellationToken)
    {
        var setting = await _dbContext.AppSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Key == AppSettingConstant.BREAST_CANCER_PREDICTION_API_URL,
                cancellationToken);

        return setting?.Value;
    }

    private static MultipartFormDataContent BuildMultipartContent(IFormFile file)
    {
        var content = new MultipartFormDataContent();

        var stream = file.OpenReadStream();
        var streamContent = new StreamContent(stream);

        streamContent.Headers.ContentType =
            new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

        content.Add(streamContent, "image_path", file.FileName);

        return content;
    }
}
