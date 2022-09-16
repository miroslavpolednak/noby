using FluentValidation;
using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CreateAssessmentAsynchronous;

internal sealed class CreateAssessmentAsynchronousValidator
    : AbstractValidator<_V2.RiskBusinessCaseCreateAssessmentAsynchronousRequest>
{
    public CreateAssessmentAsynchronousValidator()
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
            .WithErrorCode("ItChannelPrevious")
            .NotEqual(Contracts.Shared.ItChannels.Unknown)
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
