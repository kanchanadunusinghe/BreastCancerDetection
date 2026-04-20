using BCD.Application.DTOs.MachineLearningDTOs;
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

        [HttpGet("getPatientsByFilter")]
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

        [HttpGet("getPatientMasterData")]
        public async Task<IActionResult> GetPatientMasterData()
        {
            var response = await mediator.Send(new GetPatientMasterDataQuery());

            return Ok(response);
        }

        [HttpPost("breastCancerPredict")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<IActionResult> BreastCancerPredict(
            [FromForm] BreastCancerPredictionRequestDto breastCancerPredictionRequestDto)
        {
            var response = await mediator.Send(new BreastCancerPredictCommand(breastCancerPredictionRequestDto));

            return Ok(response);
        }

        [HttpPut("mammographyScanCommentUpdate")]
        public async Task<IActionResult> MammographyScanCommentUpdate(
            MammographyScanCommentUpdateCommand mammographyScanCommentUpdateCommand)
        {
            var response = await mediator.Send(mammographyScanCommentUpdateCommand);

            return Ok(response);
        }

        [HttpGet("getMammographyScansByPatinetId/{patinetId:int}")]
        public async Task<IActionResult> GetMammographyScansByPatinetId(int patinetId)
        {
            var response = await mediator.Send(new GetMammographyScansByPatinetIdQuery(patinetId));

            return Ok(response);
        }
    }
}
