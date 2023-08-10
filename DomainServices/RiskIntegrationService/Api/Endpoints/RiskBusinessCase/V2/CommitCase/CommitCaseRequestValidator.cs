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

        When(t => t.LoanAgreement != null, () =>
        {
            RuleFor(x => x.LoanAgreement)
            .ChildRules(x2 =>
            {
                x2.RuleFor(x2 => x2!.DistributionChannelId)
                .NotEmpty()
                .WithErrorCode("LoanAgreement.DistributionChannelId");

                x2.RuleFor(x2 => x2!.SignatureTypeId)
                .NotEmpty()
                .WithErrorCode("LoanAgreement.SignatureTypeId");
            });
        });
    }
}
