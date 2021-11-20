using FluentValidation;

namespace DomainServices.CaseService.Api.Validators.SalesArrangement;

internal class UpdateSalesArrangementStateMediatrRequestValidator
    : AbstractValidator<Dto.SalesArrangement.UpdateSalesArrangementStateMediatrRequest>
{
    public UpdateSalesArrangementStateMediatrRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("13000");
    }
}
