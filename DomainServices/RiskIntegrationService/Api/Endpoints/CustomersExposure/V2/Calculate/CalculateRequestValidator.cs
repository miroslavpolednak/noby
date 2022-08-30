using DomainServices.RiskIntegrationService.Api.GlobalValidators;
using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomersExposure.V2.Calculate;

internal sealed class CalculateRequestValidator
    : AbstractValidator<Contracts.CustomersExposure.V2.CustomersExposureCalculateRequest>
{
    public CalculateRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode("SalesArrangementId");

        RuleFor(t => t.RiskBusinessCaseId)
            .NotEmpty()
            .WithErrorCode("RiskBusinessCaseId");

        RuleFor(t => t.LoanApplicationDataVersion)
            .NotEmpty()
            .WithErrorCode("LoanApplicationDataVersion");

        When(t => t.UserIdentity is not null, () =>
        {
            RuleFor(t => t.UserIdentity)
                .SetValidator(new IdentityValidator())
                .WithErrorCode("UserIdentity");
        });
    }
}
