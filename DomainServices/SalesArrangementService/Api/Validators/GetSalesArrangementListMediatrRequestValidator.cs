using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class GetSalesArrangementListMediatrRequestValidator
    : AbstractValidator<Dto.GetSalesArrangementListMediatrRequest>
{
    public GetSalesArrangementListMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("18008");
    }
}
