using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class ValidateSalesArrangementMediatrRequestValidator
    : AbstractValidator<Dto.ValidateSalesArrangementMediatrRequest>
{
    public ValidateSalesArrangementMediatrRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId Id must be > 0").WithErrorCode("18010");
    }
}
