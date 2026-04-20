using BCD.Application.Common.Interfaces;
using BCD.Application.DTOs.Common;
using BCD.Application.DTOs.MachineLearningDTOs;
using BCD.Application.Pipelines.Patients.Query;
using BCD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BCD.Application.Pipelines.Patients.Command;

public record BreastCancerPredictCommand(
    BreastCancerPredictionRequestDto BreastCancerPredictionRequest) : IRequest<ResultDto<BreastCancerPredictionResultResponseDto>>;

public class BreastCancerPredictCommandHandler(
    IBCDMachineLearningService bCDMachineLearningService,
    ILogger<BreastCancerPredictCommandHandler> logger,
    IMediator mediator,
    IHttpContextAccessor httpContextAccessor,
    IBCDContext bCDContext)
    : IRequestHandler<BreastCancerPredictCommand, ResultDto<BreastCancerPredictionResultResponseDto>>
{

    public async Task<ResultDto<BreastCancerPredictionResultResponseDto>>
        Handle(BreastCancerPredictCommand request, CancellationToken cancellationToken)
    {
        if (request?.BreastCancerPredictionRequest is null)
        {
            return ResultDto<BreastCancerPredictionResultResponseDto>
                .Failure("Invalid request.");
        }

        try
        {
            var patient = await bCDContext.Patients
                .Include(p => p.MammographyScans)
                .FirstOrDefaultAsync(
                    x => x.Id == request.BreastCancerPredictionRequest.PatientId,
                    cancellationToken);

            if (patient is null)
            {
                logger.LogWarning(
                    "Breast cancer prediction failed. Patient not found. PatientId: {PatientId}",
                    request.BreastCancerPredictionRequest.PatientId);

                return ResultDto<BreastCancerPredictionResultResponseDto>
                    .Failure("Patient not found.");
            }

            var predictionResult = await bCDMachineLearningService
                .PredictBreastCancerAsync(
                    request.BreastCancerPredictionRequest.ExrayImage,
                    cancellationToken);

            if (!predictionResult.Success || predictionResult.Data is null)
            {
                logger.LogWarning(
                    "ML prediction failed for PatientId: {PatientId}. Reason: {Reason}",
                    patient.Id,
                    predictionResult.Message);

                return ResultDto<BreastCancerPredictionResultResponseDto>
                    .Failure(predictionResult.Message ?? "Prediction failed.");
            }

            var savedImagePath = await SaveFileAsync(
                                request.BreastCancerPredictionRequest.ExrayImage,
                                patient.Id,
                                cancellationToken);



            var mammographyScan = new MammographyScan
            {
                PatientId = patient.Id,
                ConfidenceScore = predictionResult.Data.Probability,
                PredictionResult = predictionResult.Data.CancerType,
                CreatedAt = DateTime.Now,
                ImageUrl = savedImagePath, // TODO: Save file to storage and persist URL,
                CreatedUserId = request.BreastCancerPredictionRequest.LoggedInUserId
            };


            await bCDContext.MammographyScans.AddAsync(mammographyScan);

            await bCDContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Breast cancer prediction saved successfully. PatientId: {PatientId}, Result: {Result}, Confidence: {Confidence}",
                patient.Id,
                mammographyScan.PredictionResult,
                mammographyScan.ConfidenceScore);

            var patientDetail = await mediator.Send(new GetPatientByIdQuery(patient.Id));


            var mammographyScanDetail = await bCDContext.MammographyScans
                                        .Include(x => x.CreatedUser)
                                        .FirstOrDefaultAsync(x => x.Id == mammographyScan.Id);

            var imageUrl = string.Empty;

            if (!string.IsNullOrEmpty(mammographyScanDetail?.ImageUrl))
            {
                var fileName = Path.GetFileName(mammographyScanDetail.ImageUrl);
                var folderName = new DirectoryInfo(
                    Path.GetDirectoryName(mammographyScanDetail.ImageUrl)!
                ).Name;

                var httpRequest = httpContextAccessor.HttpContext!.Request;

                var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}";

                imageUrl = $"{baseUrl}/mammography-images/{folderName}/{fileName}";
            }

            var breastCancerPredictionResultResponseDto = new BreastCancerPredictionResultResponseDto
            {
                CancerType = predictionResult?.Data?.CancerType ?? string.Empty,
                Probability = predictionResult?.Data?.Probability ?? 0,
                Patient = patientDetail,
                CreatedAt = mammographyScanDetail!.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                ExrayImageUrl = imageUrl ?? string.Empty,
                CreatedUserName = mammographyScanDetail?.CreatedUser != null
                              ? $"{mammographyScanDetail.CreatedUser.FirstName} {mammographyScanDetail.CreatedUser.LastName}"
                              : string.Empty,
                RecordId = mammographyScanDetail?.Id ?? 0
            };

            return ResultDto<BreastCancerPredictionResultResponseDto>
                .Ok(breastCancerPredictionResultResponseDto);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("Breast cancer prediction request was cancelled.");
            return ResultDto<BreastCancerPredictionResultResponseDto>
                .Failure("Request was cancelled.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error occurred while processing breast cancer prediction.");

            return ResultDto<BreastCancerPredictionResultResponseDto>
                .Failure("An unexpected system error occurred.");
        }
    }

    private async Task<string> SaveFileAsync(IFormFile file, int patientId, CancellationToken cancellationToken)
    {
        var rootPath = @"C:\BCDMammographyImages";

        // Create patient folder path
        var patientFolder = Path.Combine(rootPath, patientId.ToString());

        if (!Directory.Exists(patientFolder))
        {
            Directory.CreateDirectory(patientFolder);
        }

        // Generate unique file name
        var fileExtension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid()}{fileExtension}";

        var fullPath = Path.Combine(patientFolder, uniqueFileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return fullPath; // you can store full path or relative path
    }
}