using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.UpdateCaseState;

internal class UpdateCaseStateRequestValidator : AbstractValidator<UpdateCaseStateRequest>
{
    public UpdateCaseStateRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13016");

        RuleFor(t => t.State)
            .GreaterThan(0)
            .WithMessage("Case State must be > 0").WithErrorCode("13017");
    }
}
