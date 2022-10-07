using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class SendToCmpMediatrRequestValidator
    : AbstractValidator<Dto.SendToCmpMediatrRequest>
{
    public SendToCmpMediatrRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId Id must be > 0").WithErrorCode("18010");
    }
}
