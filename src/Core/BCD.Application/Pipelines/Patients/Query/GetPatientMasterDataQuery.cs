using BCD.Application.Common.Enums;
using BCD.Application.Common.Utility;
using BCD.Application.DTOs.PatientDTOs;
using BCD.Domain.Enums;
using MediatR;

namespace BCD.Application.Pipelines.Patients.Query;

public record GetPatientMasterDataQuery() : IRequest<PatientMasterDataDto>;

public class GetPatientMasterDataQueryHandler : IRequestHandler<GetPatientMasterDataQuery, PatientMasterDataDto>
{
    public async Task<PatientMasterDataDto> Handle(GetPatientMasterDataQuery request, CancellationToken cancellationToken)
    {
        var listOfGenders = EnumHelper.GetDropDownList<Gender>();

        var listOfListOfActiveStatus = EnumHelper.GetDropDownList<ActiveStatus>();

        return new PatientMasterDataDto(listOfGenders, listOfListOfActiveStatus);
    }
}