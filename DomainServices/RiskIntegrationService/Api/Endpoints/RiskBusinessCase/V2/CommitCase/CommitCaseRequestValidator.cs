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
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleFor(t => t.ProductTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        RuleFor(t => t.FinalResult)
            .IsInEnum()
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError)
            .NotEqual(_V2.RiskBusinessCaseFinalResults.Unknown)
            .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

        When(t => t.LoanAgreement != null, () =>
        {
            RuleFor(x => x.LoanAgreement)
            .ChildRules(x2 =>
            {
                x2.RuleFor(x2 => x2!.DistributionChannelId)
                .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.GeneralValidationError);

                x2.RuleFor(x2 => x2!.SignatureTypeId)
                .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.GeneralValidationError);
            });
        });
    }
}
