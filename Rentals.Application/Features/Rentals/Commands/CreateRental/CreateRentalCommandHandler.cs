using EnterpriseTravelBooking.Shared.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rentals.Application.Interfaces;
using Rentals.Domain.Entities;
using System.Security.Claims;

namespace Rentals.Application.Features.Rentals.Commands.CreateRental
{
    public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, Guid>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CreateRentalCommandHandler> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateRentalCommandHandler(
            IRentalRepository rentalRepository,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CreateRentalCommandHandler> logger,
            IPublishEndpoint publishEndpoint)
        {
            _rentalRepository = rentalRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userIdString = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var customerEmail = user?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userIdString))
                throw new UnauthorizedAccessException("Giriş yapmalısınız.");

            var vehicle = await _rentalRepository.GetVehicleByIdAsync(request.VehicleId);
            if (vehicle is null)
                throw new KeyNotFoundException("Araç bulunamadı.");

            var startUtc = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc);
            var endUtc = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc);

            var isOverlapping = await _rentalRepository.HasOverlappingRentalAsync(
                request.VehicleId, startUtc, endUtc);

            if (isOverlapping)
                throw new InvalidOperationException(
                    "Araç seçtiğiniz tarihler arasında başka bir kiralama için ayrılmış.");

            var totalDays = Math.Max((endUtc - startUtc).Days, 1);

            var rental = new Rental
            {
                Id = Guid.NewGuid(),
                VehicleId = request.VehicleId,
                CustomerId = userIdString,
                StartDate = startUtc,
                EndDate = endUtc,
                TotalPrice = totalDays * vehicle.DailyPrice,
                Status = RentalStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            var rentalId = await _rentalRepository.CreateRentalTransactionAsync(rental, vehicle);

            await _publishEndpoint.Publish(new RentalCreatedEvent
            {
                RentalId = rental.Id,
                CustomerEmail = customerEmail ?? string.Empty,
                VehicleInfo = $"{vehicle.Id} nolu araç",
                TotalPrice = rental.TotalPrice,
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);

            _logger.LogInformation("Kiralama oluşturuldu. RentalId: {RentalId}", rental.Id);

            return rentalId;
        }
    }
}
