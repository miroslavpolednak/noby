using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class UpdateSalesArrangementStateMediatrRequestValidator
    : AbstractValidator<Dto.UpdateSalesArrangementStateMediatrRequest>
{
    public UpdateSalesArrangementStateMediatrRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("13000");
    }
}
