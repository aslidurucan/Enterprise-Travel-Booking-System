using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rentals.API.Application.Features.Rentals.Commands.CreateRental;
using Rentals.API.Application.Features.Rentals.Queries.GetAllRentals;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public async Task<IActionResult> GetAllRentals()
        {
            var query = new GetAllRentalsQuery();
            var rentals = await _mediator.Send(query);

            return Ok(rentals);
        }


    }
}
