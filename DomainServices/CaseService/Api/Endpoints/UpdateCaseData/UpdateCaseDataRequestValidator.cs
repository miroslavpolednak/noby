using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.UpdateCaseData;

internal sealed class UpdateCaseDataRequestValidator
    : AbstractValidator<UpdateCaseDataRequest>
{
    public UpdateCaseDataRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13016");

        RuleFor(t => t.Data.ProductTypeId)
            .GreaterThan(0)
            .WithMessage(t => "ProductTypeId must be > 0").WithErrorCode("13002");

        RuleFor(t => (decimal)t.Data.TargetAmount)
            .GreaterThan(0)
            .WithMessage("Target amount must be > 0").WithErrorCode("13018");
    }
}
