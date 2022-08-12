using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class UpdateSalesArrangementMediatrRequestValidator
    : AbstractValidator<Dto.UpdateSalesArrangementMediatrRequest>
{
    public UpdateSalesArrangementMediatrRequestValidator()
    {
        RuleFor(t => t.Request.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("16010");
    }
}
