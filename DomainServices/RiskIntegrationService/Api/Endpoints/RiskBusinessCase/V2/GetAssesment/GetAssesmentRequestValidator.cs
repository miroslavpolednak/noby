using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.GetAssesment;

internal sealed class GetAssesmentRequestValidator
    : AbstractValidator<Contracts.RiskBusinessCase.V2.RiskBusinessCaseGetAssesmentRequest>
{
    public GetAssesmentRequestValidator()
    {
        RuleFor(t => t.LoanApplicationAssessmentId)
            .NotEmpty()
            .WithErrorCode("LoanApplicationAssessmentId");
    }
}
