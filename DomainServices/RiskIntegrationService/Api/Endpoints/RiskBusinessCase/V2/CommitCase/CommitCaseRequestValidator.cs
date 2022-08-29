using FluentValidation;
using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CommitCase;

internal sealed class CommitCaseValidator
    : AbstractValidator<_V2.RiskBusinessCaseCommitCaseRequest>
{
    public CommitCaseValidator()
    {
        RuleFor(t => t.RiskBusinessCaseId)
            .NotEmpty()
            .WithErrorCode("RiskBusinessCaseId");

        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode("SalesArrangementId");

        RuleFor(t => t.ProductTypeId)
            .GreaterThan(0)
            .WithErrorCode("ProductTypeId");

        RuleFor(t => t.FinalResult)
            .IsInEnum()
            .WithErrorCode("FinalResult")
            .NotEqual(_V2.RiskBusinessCaseFinalResults.Unknown)
            .WithErrorCode("FinalResult");
    }
}
