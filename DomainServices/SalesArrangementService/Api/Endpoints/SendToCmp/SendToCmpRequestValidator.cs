using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.SendToCmp;

internal class SendToCmpRequestValidator
    : AbstractValidator<Contracts.SendToCmpRequest>
{
    public SendToCmpRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId Id must be > 0").WithErrorCode("18010");
    }
}
