using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rentals.Application.Features.Rentals.Commands.CreateRental;
using Rentals.Application.Features.Rentals.Queries.GetAllRentals;

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
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CreateRentalCommand command)
        {
            var rentalId = await _mediator.Send(command);
            return Ok(new { Message = "Başarılı!", RentalId = rentalId });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllRentals()
        {
            var rentals = await _mediator.Send(new GetAllRentalsQuery());
            return Ok(rentals);
        }
    }
}
