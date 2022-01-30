using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class UpdateSalesArrangementStateMediatrRequestValidator
    : AbstractValidator<Dto.UpdateSalesArrangementStateMediatrRequest>
{
    public UpdateSalesArrangementStateMediatrRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("Sales arrangement ID does not exist.").WithErrorCode("16000");

        RuleFor(t => t.State)
            .GreaterThan(0)
            .WithMessage("SalesArrangement State does not exist.").WithErrorCode("16006");
    }
}
