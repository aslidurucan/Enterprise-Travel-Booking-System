using Catalog.Application.Features.Vehicles.Commands.CreateVehicle;
using Catalog.Application.Features.Vehicles.Commands.DeleteVehicle;
using Catalog.Application.Features.Vehicles.Commands.UpdateVehicle;
using Catalog.Application.Features.Vehicles.Queries.GetAllVehicles;
using Catalog.Application.Features.Vehicles.Queries.GetPagedVehicles;
using Catalog.Application.Features.Vehicles.Queries.GetVehicleById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehiclesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleCommand command)
        {
            var vehicleId = await _mediator.Send(command);
            return Ok(new { Id = vehicleId, Message = "Araç başarıyla eklendi!" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await _mediator.Send(new GetAllVehiclesQuery());

            return Ok(vehicles);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateVehicle([FromBody] UpdateVehicleCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = "Araç başarıyla güncellendi!" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            var command = new DeleteVehicleCommand { Id = id };
            await _mediator.Send(command);
            return Ok(new { Message = "Araç başarıyla silindi!" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(Guid id)
        {
            var query = new GetVehicleByIdQuery { Id = id };

            var result = await _mediator.Send(query);

            return Ok(result);
        }
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedVehicles([FromQuery] GetPagedVehiclesQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

    }
}

