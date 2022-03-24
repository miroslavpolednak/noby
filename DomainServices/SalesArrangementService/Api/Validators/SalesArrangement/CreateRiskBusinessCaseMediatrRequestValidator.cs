using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators.SalesArrangement;

internal class CreateRiskBusinessCaseMediatrRequestValidator
    : AbstractValidator<Dto.CreateRiskBusinessCaseMediatrRequest>
{
    public CreateRiskBusinessCaseMediatrRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangement ID does not exist.").WithErrorCode("16000");
    }
}
