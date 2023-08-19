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
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleFor(t => t.LoanApplicationDataVersion)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleFor(t => t.ItChannelPrevious)
            .IsInEnum()
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleFor(t => t.AssessmentMode)
            .IsInEnum()
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError)
            .NotEqual(_V2.RiskBusinessCaseAssessmentModes.Unknown)
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleFor(t => t.GrantingProcedureCode)
            .IsInEnum()
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError)
            .NotEqual(_V2.RiskBusinessCaseGrantingProcedureCodes.Unknown)
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);
    }
}
