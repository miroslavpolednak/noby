using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.GetAssessment;

internal sealed class GetAssessmentRequestValidator
    : AbstractValidator<Contracts.RiskBusinessCase.V2.RiskBusinessCaseGetAssessmentRequest>
{
    public GetAssessmentRequestValidator()
    {
        RuleFor(t => t.LoanApplicationAssessmentId)
            .NotEmpty()
            .WithErrorCode("LoanApplicationAssessmentId");
    }
}
