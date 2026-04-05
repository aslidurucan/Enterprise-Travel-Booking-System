using FluentValidation;

namespace Catalog.Application.Features.Vehicles.Queries.GetPagedVehicles
{
    public class GetPagedVehiclesQueryValidator : AbstractValidator<GetPagedVehiclesQuery>
    {
        public GetPagedVehiclesQueryValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(1).WithMessage("Sayfa numarası 1'den küçük olamaz.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Sayfa boyutu 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(50).WithMessage("Sayfa boyutu en fazla 50 olabilir.");
        }
    }
}
