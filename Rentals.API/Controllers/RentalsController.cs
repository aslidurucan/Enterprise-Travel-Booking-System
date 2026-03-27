using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rentals.API.Application.Features.Rentals.Commands.CreateRental;

namespace Rentals.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RentalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalCommand command)
        {
            var rentalId = await _mediator.Send(command);

            return Ok(new
            {
                Message = "Kiralama işlemi başarıyla tamamlandı!",
                RentalId = rentalId
            });
        }

    }
}
