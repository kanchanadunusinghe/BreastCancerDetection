using BCD.Application.Common.Interfaces;
using BCD.Application.DTOs.Common;
using BCD.Application.DTOs.PatientDTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BCD.Application.Pipelines.Patients.Query;

public record GetPatientByFilterQuery(PatientFilterDto patientFilterDto)
    : IRequest<PagedResultDto<PatientListDto>>;

public class GetPatientByFilterQueryHandler
    (IBCDContext bCDContext) : IRequestHandler<GetPatientByFilterQuery, PagedResultDto<PatientListDto>>
{
    public async Task<PagedResultDto<PatientListDto>> Handle(GetPatientByFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.patientFilterDto;

        var query = bCDContext.Patients
                    .AsNoTracking()
                    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(x =>
                x.FirstName.Contains(filter.Search) ||
                x.LastName.Contains(filter.Search) ||
                x.NHSNumber.Contains(filter.Search) ||
                x.Email.Contains(filter.Search) ||
                x.PostCode.Contains(filter.Search));
        }

        if (filter.Gender != 0)
        {
            query = query.Where(x => x.Gender == filter.Gender);
        }


        var totalCount = await query.CountAsync(cancellationToken);

        var patients = await query
                  .OrderByDescending(x => x.CreatedAt)
                  .Skip((filter.PageNumber - 1) * filter.PageSize)
                  .Take(filter.PageSize)
                  .Select(x => new PatientListDto
                  {
                      Id = x.Id,
                      NHSNumber = x.NHSNumber,
                      FirstName = x.FirstName,
                      LastName = x.LastName,
                      Email = x.Email,
                      MobileNumber = x.MobileNumber,
                      DateOfBirth = x.DateOfBirth,
                      Gender = x.Gender.ToString(),
                      PostCode = x.PostCode,
                      CreatedAt = x.CreatedAt,
                      MammographyScanCount = x.MammographyScans.Count()
                  })
                  .ToListAsync(cancellationToken);


        return new PagedResultDto<PatientListDto>
        {
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount,
            Data = patients
        };
    }
}
