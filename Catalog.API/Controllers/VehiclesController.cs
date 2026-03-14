using Catalog.Application.Features.Vehicles.Commands.CreateVehicle;
using Catalog.Application.Features.Vehicles.Commands.DeleteVehicle;
using Catalog.Application.Features.Vehicles.Commands.UpdateVehicle;
using Catalog.Application.Features.Vehicles.Queries.GetAllVehicles;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPut]
        public async Task<IActionResult> UpdateVehicle([FromBody] UpdateVehicleCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = "Araç başarıyla güncellendi!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            var command = new DeleteVehicleCommand { Id = id };
            await _mediator.Send(command);
            return Ok(new { Message = "Araç başarıyla silindi!" });
        }
    }
}
