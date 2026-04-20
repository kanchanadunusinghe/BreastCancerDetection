using BCD.Application.DTOs.Common;

namespace BCD.Application.DTOs.PatientDTOs;

public record PatientMasterDataDto(
    List<DropDownDto> ListOfGenders,
    List<DropDownDto> ListOfActiveStatus);

