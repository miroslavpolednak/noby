using FluentValidation;

namespace DomainServices.CaseService.Api.Validators.SalesArrangement;

internal class UpdateSalesArrangementDataMediatrRequestValidator
    : AbstractValidator<Dto.SalesArrangement.UpdateSalesArrangementDataMediatrRequest>
{
    public UpdateSalesArrangementDataMediatrRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("13000");
    }
}
