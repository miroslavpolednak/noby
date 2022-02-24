using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class GetSalesArrangementDetailMediatrRequestValidator
    : AbstractValidator<Dto.GetSalesArrangementMediatrRequest>
{
    public GetSalesArrangementDetailMediatrRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("13000");
    }
}
