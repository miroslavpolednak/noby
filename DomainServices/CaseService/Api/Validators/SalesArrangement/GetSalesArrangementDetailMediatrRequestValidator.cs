using FluentValidation;

namespace DomainServices.CaseService.Api.Validators.SalesArrangement;

internal class GetSalesArrangementDetailMediatrRequestValidator
    : AbstractValidator<Dto.SalesArrangement.GetSalesArrangementDetailMediatrRequest>
{
    public GetSalesArrangementDetailMediatrRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("13000");
    }
}
