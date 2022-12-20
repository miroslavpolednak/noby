using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.DeleteCase;

internal sealed class DeleteCaseRequestValidator
    : AbstractValidator<DeleteCaseRequest>
{
    public DeleteCaseRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13016");
    }
}
