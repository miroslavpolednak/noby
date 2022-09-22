using FluentValidation;
using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateAssessment;

internal sealed class CreateAssessmentValidator
    : AbstractValidator<_V2.RiskBusinessCaseCreateAssessmentRequest>
{
    public CreateAssessmentValidator()
    {
        RuleFor(t => t.RiskBusinessCaseId)
            .NotEmpty()
            .WithErrorCode("RiskBusinessCaseId");

        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode("SalesArrangementId");

        RuleFor(t => t.LoanApplicationDataVersion)
            .NotEmpty()
            .WithErrorCode("LoanApplicationDataVersion");

        RuleFor(t => t.ItChannelPrevious)
            .IsInEnum()
            .WithErrorCode("ItChannelPrevious");

        RuleFor(t => t.AssessmentMode)
            .IsInEnum()
            .WithErrorCode("AssessmentMode")
            .NotEqual(_V2.RiskBusinessCaseAssessmentModes.Unknown)
            .WithErrorCode("AssessmentMode");

        RuleFor(t => t.GrantingProcedureCode)
            .IsInEnum()
            .WithErrorCode("GrantingProcedureCode")
            .NotEqual(_V2.RiskBusinessCaseGrantingProcedureCodes.Unknown)
            .WithErrorCode("GrantingProcedureCode");
    }
}
