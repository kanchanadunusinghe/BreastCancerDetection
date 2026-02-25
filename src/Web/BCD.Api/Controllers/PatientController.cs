using BCD.Application.DTOs.PatientDTOs;
using BCD.Application.Pipelines.Patients.Command;
using BCD.Application.Pipelines.Patients.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BCD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController(IMediator mediator) : ControllerBase
    {
        [HttpGet("getPatientById/{id:int}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var response = await mediator.Send(new GetPatientByIdQuery(id));

            return Ok(response);
        }

        [HttpGet("getPatientByFilter")]
        public async Task<IActionResult> GetPatientByFilter([FromQuery] PatientFilterDto patientFilterDto)
        {
            var response = await mediator.Send(new GetPatientByFilterQuery(patientFilterDto));

            return Ok(response);
        }

        [HttpPost("savePatient")]
        public async Task<IActionResult> SavePatient([FromBody] PatientRequestDto patientRequestDto)
        {
            var response = await mediator.Send(new SavePatientCommand(patientRequestDto));

            return Ok(response);
        }
    }
}
