using FluentValidation;

namespace Catalog.Application.Features.Vehicles.Commands.UpdateVehicle
{
    public class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
    {
        public UpdateVehicleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Güncellenecek araç ID'si boş olamaz.");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Araç markası boş bırakılamaz.")
                .MinimumLength(2).WithMessage("Araç markası en az 2 karakter olmalıdır.");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Araç modeli boş bırakılamaz.")
                .MinimumLength(2).WithMessage("Araç modeli en az 2 karakter olmalıdır.");

            RuleFor(x => x.DailyPrice)
                .GreaterThan(0).WithMessage("Günlük kiralama bedeli 0'dan büyük olmalıdır.");

            RuleFor(x => x.Year)
                .GreaterThanOrEqualTo(1990).WithMessage("Araç yılı 1990'dan eski olamaz.")
                .LessThanOrEqualTo(DateTime.Now.Year + 1).WithMessage("Araç yılı gelecekte bir yıl olamaz.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Para birimi boş olamaz.")
                .Length(3).WithMessage("Para birimi tam olarak 3 karakter olmalıdır (Örn: TRY, USD).");
        }
    }
}
