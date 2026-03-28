using MediatR;
using Rentals.API.Application.Features.Rentals.Commands.CreateRental;
using Rentals.API.Application.Interfaces;
using Rentals.API.Domain.Entities;

public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, Guid>
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CreateRentalCommandHandler> _logger;

    public CreateRentalCommandHandler(
        IRentalRepository rentalRepository,
        IHttpContextAccessor httpContextAccessor,
        ILogger<CreateRentalCommandHandler> logger)
    {
        _rentalRepository = rentalRepository;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user != null)
        {
            foreach (var claim in user.Claims)
            {
                _logger.LogInformation($"[DEBUG] Claim Type: {claim.Type}, Value: {claim.Value}");
            }
        }
        var userIdString = user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                           ?? user?.FindFirst("sub")?.Value
                           ?? user?.FindFirst("id")?.Value;

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            _logger.LogWarning("Kullanıcı ID'si Token içinde bulunamadı veya geçersiz!");
            throw new UnauthorizedAccessException("Giriş yapmalısınız!");
        }

        var vehicle = await _rentalRepository.GetVehicleByIdAsync(request.VehicleId);
        if (vehicle == null || !vehicle.IsAvailable)
            throw new Exception("Araç müsait değil!");

        if (request.StartDate >= request.EndDate)
            throw new Exception("Tarihler hatalı!");

        var totalDays = Math.Max((request.EndDate - request.StartDate).Days, 1);

        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            VehicleId = request.VehicleId,
            CustomerId = userIdString,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalPrice = totalDays * vehicle.DailyPrice,
            Status = RentalStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        return await _rentalRepository.CreateRentalTransactionAsync(rental, vehicle);
    }
}