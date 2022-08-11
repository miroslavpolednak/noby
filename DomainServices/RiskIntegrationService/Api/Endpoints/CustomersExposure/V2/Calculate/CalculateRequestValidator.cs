using DomainServices.RiskIntegrationService.Api.GlobalValidators;
using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CustomersExposure.V2.Calculate;

internal sealed class CalculateRequestValidator
    : AbstractValidator<Contracts.CustomersExposure.V2.CustomersExposureCalculateRequest>
{
    public CalculateRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0);

        RuleFor(t => t.RiskBusinessCaseId)
            .NotEmpty();

        RuleFor(t => t.LoanApplicationDataVersion)
            .NotEmpty();

        When(t => t.UserIdentity is not null, () =>
        {
            RuleFor(t => t.UserIdentity)
            .SetValidator(new IdentityValidator());
        });
    }
}
