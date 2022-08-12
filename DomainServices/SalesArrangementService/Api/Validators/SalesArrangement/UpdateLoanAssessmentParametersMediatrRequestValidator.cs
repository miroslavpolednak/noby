using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class UpdateLoanAssessmentParametersMediatrRequestValidator
    : AbstractValidator<Dto.UpdateLoanAssessmentParametersMediatrRequest>
{
    public UpdateLoanAssessmentParametersMediatrRequestValidator()
    {
        RuleFor(t => t.Request.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("16010");
    }
}
