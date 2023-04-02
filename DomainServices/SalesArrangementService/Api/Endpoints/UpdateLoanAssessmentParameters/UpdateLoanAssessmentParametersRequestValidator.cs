using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdateLoanAssessmentParameters;

internal sealed class UpdateLoanAssessmentParametersRequestValidator
    : AbstractValidator<Contracts.UpdateLoanAssessmentParametersRequest>
{
    public UpdateLoanAssessmentParametersRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("18010");
    }
}
