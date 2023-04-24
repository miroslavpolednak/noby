using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.CreateTask;

internal sealed class CreateTaskRequestValidator
    : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(t => t.TaskTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.TaskTypeIdIsEmpty)
            .Must(t => t == 3 || t == 7)
            .WithErrorCode(ErrorCodeMapper.TaskTypeIdNotAllowed);

        RuleFor(t => t.ProcessId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.ProcessIdIsEmpty);
    }
}
