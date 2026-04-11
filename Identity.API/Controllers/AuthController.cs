using Identity.Application.Features.Auth.Commands.Login;
using Identity.Application.Features.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);

            return StatusCode(StatusCodes.Status201Created, new
            {
                result.UserId,
                result.Username,
                result.Email,
                result.Role,
                Message = "Kayıt başarılı! Artık giriş yapabilirsiniz."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(new
            {
                result.Token,
                result.Username,
                result.Role,
                Message = "Giriş başarılı!"
            });
        }
    }
}
