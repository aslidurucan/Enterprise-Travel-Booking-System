using FluentValidation;

namespace Catalog.Application.Features.Vehicles.Commands.DeleteVehicle
{
    public class DeleteVehicleCommandValidator : AbstractValidator<DeleteVehicleCommand>
    {
        public DeleteVehicleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Silinecek araç ID'si boş olamaz.");
        }
    }
}
