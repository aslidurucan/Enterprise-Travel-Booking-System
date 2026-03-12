using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Vehicles.Commands.CreateVehicle
{
    public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
    {
        public CreateVehicleCommandValidator()
        {
            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Araç markası boş bırakılamaz.")
                .MinimumLength(2).WithMessage("Araç markası en az 2 karakter olmalıdır.");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Araç modeli boş bırakılamaz.");

            RuleFor(x => x.DailyPrice)
                .GreaterThan(0).WithMessage("Günlük kiralama bedeli 0'dan büyük olmalıdır.");

            RuleFor(x => x.Year)
                .GreaterThanOrEqualTo(1990).WithMessage("Araç yılı 1990'dan eski olamaz.")
                .LessThanOrEqualTo(DateTime.Now.Year + 1).WithMessage("Araç yılı gelecekte bir yıl olamaz.");
        }
    }
}
