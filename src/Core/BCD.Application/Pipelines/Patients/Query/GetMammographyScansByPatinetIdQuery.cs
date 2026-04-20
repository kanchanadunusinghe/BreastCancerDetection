using BCD.Application.Common.Interfaces;
using BCD.Application.DTOs.PatientDTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BCD.Application.Pipelines.Patients.Query;

public record GetMammographyScansByPatinetIdQuery(int PatinetId)
    : IRequest<List<MammographyScanResultDto>>;

public class GetMammographyScansByPatinetIdQueryHandler(
    IBCDContext bCDContext,
    IHttpContextAccessor httpContextAccessor
    )
    : IRequestHandler<GetMammographyScansByPatinetIdQuery, List<MammographyScanResultDto>>
{
    public async Task<List<MammographyScanResultDto>> Handle(
        GetMammographyScansByPatinetIdQuery request,
        CancellationToken cancellationToken)
    {
        var patient = await bCDContext.Patients
               .AsNoTracking()
               .Include(p => p.MammographyScans)
                   .ThenInclude(ms => ms.CreatedUser)
               .FirstOrDefaultAsync(p => p.Id == request.PatinetId, cancellationToken);

        if (patient == null)
            return new List<MammographyScanResultDto>();

        var httpRequest = httpContextAccessor.HttpContext?.Request;
        var baseUrl = httpRequest != null
            ? $"{httpRequest.Scheme}://{httpRequest.Host}"
            : string.Empty;

        var results = patient.MammographyScans
            .OrderByDescending(ms => ms.Id)
            .Select(ms =>
            {
                var imageUrl = BuildImageUrl(ms.ImageUrl, baseUrl);

                return new MammographyScanResultDto
                {
                    Id = ms.Id,
                    RecordId = $"##{ms.Id}",
                    CancerType = ms.PredictionResult,
                    Comment = ms.Comment,
                    CreatedAt = ms.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreatedUserName = ms.CreatedUser != null
                        ? $"{ms.CreatedUser.FirstName} {ms.CreatedUser.LastName}"
                        : string.Empty,
                    ExrayImageUrl = imageUrl,
                    Probability = ms.ConfidenceScore,
                    Patient = new PatientRequestDto
                    {
                        Id = patient.Id,
                        NHSNumber = patient.NHSNumber,
                        FirstName = patient.FirstName,
                        LastName = patient.LastName,
                        Email = patient.Email,
                        MobileNumber = patient.MobileNumber,
                        DateOfBirth = patient.DateOfBirth,
                        Gender = patient.Gender,
                        PostCode = patient.PostCode
                    }
                };
            })
            .ToList();

        return results;
    }

    private static string BuildImageUrl(string? imagePath, string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(imagePath) || string.IsNullOrWhiteSpace(baseUrl))
            return string.Empty;

        var fileName = Path.GetFileName(imagePath);
        var directoryPath = Path.GetDirectoryName(imagePath);

        if (string.IsNullOrEmpty(directoryPath))
            return string.Empty;

        var folderName = new DirectoryInfo(directoryPath).Name;

        return $"{baseUrl}/mammography-images/{folderName}/{fileName}";
    }
}
