using FluentValidation;

namespace Rentals.Application.Features.Rentals.Commands.CreateRental
{
    public class CreateRentalCommandValidator : AbstractValidator<CreateRentalCommand>
    {
        public CreateRentalCommandValidator()
        {
            RuleFor(x => x.VehicleId)
                .NotEmpty()
                .WithMessage("Araç seçimi zorunludur.");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Başlangıç tarihi zorunludur.")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Başlangıç tarihi bugünden önce olamaz.");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("Bitiş tarihi zorunludur.")
                .GreaterThan(x => x.StartDate)
                .WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır.");

            RuleFor(x => x)
                .Must(x => (x.EndDate - x.StartDate).TotalDays <= 30)
                .WithMessage("Kiralama süresi en fazla 30 gün olabilir.")
                .OverridePropertyName("EndDate");
        }
    }
}
