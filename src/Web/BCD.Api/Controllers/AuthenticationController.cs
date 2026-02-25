using BCD.Application.DTOs.AuthenticationDTOs;
using BCD.Application.Pipelines.Authentication.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BCD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IMediator mediator) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationDto dto)
        {
            var response = await mediator.Send(new AuthenticationCommand(dto));

            return Ok(response);
        }
    }
}
