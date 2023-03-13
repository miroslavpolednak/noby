using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.UpdateCaseState;

internal sealed class UpdateCaseStateRequestValidator 
    : AbstractValidator<UpdateCaseStateRequest>
{
    public UpdateCaseStateRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);

        RuleFor(t => t.State)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidCaseState);
    }
}
