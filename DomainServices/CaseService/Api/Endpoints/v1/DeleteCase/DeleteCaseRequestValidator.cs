using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.v1.DeleteCase;

internal sealed class DeleteCaseRequestValidator
    : AbstractValidator<DeleteCaseRequest>
{
    public DeleteCaseRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);
    }
}
