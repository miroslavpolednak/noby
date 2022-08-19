using DomainServices.RiskIntegrationService.Api.GlobalValidators;
using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal sealed class SaveRequestValidator
    : AbstractValidator<Contracts.LoanApplication.V2.LoanApplicationSaveRequest>
{
    public SaveRequestValidator()
    {
    }
}
