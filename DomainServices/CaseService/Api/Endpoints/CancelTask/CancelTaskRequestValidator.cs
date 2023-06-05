using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.CancelTask;

internal sealed class CancelTaskRequestValidator
    : AbstractValidator<CancelTaskRequest>
{
    public CancelTaskRequestValidator()
    {
        RuleFor(t => t.TaskIdSB)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.TaskIdSBIsEmpty);
    }
}
