using BCD.Application.Common.Interfaces;
using BCD.Application.DTOs.PatientDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCD.Application.Pipelines.Patients.Query
{
    public record GetPatientByIdQuery(int Id) : IRequest<PatientRequestDto>;

    public class GetPatientByIdQueryHandler(IBCDContext bCDContext)
        : IRequestHandler<GetPatientByIdQuery, PatientRequestDto>
    {
        public async Task<PatientRequestDto> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            var patient = await bCDContext.Patients
                                  .AsNoTracking()
                                  .Where(x => x.Id == request.Id)
                                  .Select(x => new PatientRequestDto
                                  {
                                      Id = x.Id,
                                      NHSNumber = x.NHSNumber,
                                      FirstName = x.FirstName,
                                      LastName = x.LastName,
                                      Email = x.Email,
                                      MobileNumber = x.MobileNumber,
                                      DateOfBirth = x.DateOfBirth,
                                      Gender = x.Gender,
                                      PostCode = x.PostCode
                                  })
                                  .FirstOrDefaultAsync(cancellationToken);

            if (patient == null)
            {
                throw new KeyNotFoundException($"Patient with Id {request.Id} not found.");
            }

            return patient;
        }
    }
}
