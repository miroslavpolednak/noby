using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class UpdateSalesArrangementDataMediatrRequestValidator
    : AbstractValidator<Dto.UpdateSalesArrangementDataMediatrRequest>
{
    public UpdateSalesArrangementDataMediatrRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("13000");
    }
}
