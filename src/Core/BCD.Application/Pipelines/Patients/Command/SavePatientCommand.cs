using BCD.Application.Common.Interfaces;
using BCD.Application.DTOs.Common;
using BCD.Application.DTOs.PatientDTOs;
using BCD.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCD.Application.Pipelines.Patients.Command;

public record SavePatientCommand(PatientRequestDto PatientRequestDto) : IRequest<ResultDto<int>>;

public class SavePatientCommandHandler(
    IBCDContext bCDContext) : IRequestHandler<SavePatientCommand, ResultDto<int>>
{
    public async Task<ResultDto<int>> Handle(SavePatientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var dto = request.PatientRequestDto;

            Patient? patient = null;

            if (dto.Id > 0)
            {
                patient = await bCDContext.Patients
                    .FirstOrDefaultAsync(x => x.Id == dto.Id, cancellationToken);

                if (patient == null)
                {
                    return ResultDto<int>.Failure($"Patient with Id {dto.Id} not found.");
                }
            }


            if (patient == null)
            {
                patient = new Patient
                {
                    NHSNumber = dto.NHSNumber,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    MobileNumber = dto.MobileNumber,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender,
                    PostCode = dto.PostCode,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                await bCDContext.Patients.AddAsync(patient, cancellationToken);
            }
            else
            {
                patient.NHSNumber = dto.NHSNumber;
                patient.FirstName = dto.FirstName;
                patient.LastName = dto.LastName;
                patient.Email = dto.Email;
                patient.MobileNumber = dto.MobileNumber;
                patient.DateOfBirth = dto.DateOfBirth;
                patient.Gender = dto.Gender;
                patient.PostCode = dto.PostCode;
            }

            await bCDContext.SaveChangesAsync(cancellationToken);

            return ResultDto<int>.Ok(patient.Id, "Patient saved successfully.");
        }
        catch (Exception ex)
        {
            return ResultDto<int>.Failure("An error occurred while saving patient.",
                new List<string> { ex.Message });
        }
    }
}