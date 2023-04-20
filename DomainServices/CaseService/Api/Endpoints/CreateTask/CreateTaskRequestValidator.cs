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
            .WithErrorCode(ErrorCodeMapper.TaskTypeIdIsEmpty);

        RuleFor(t => t.TaskTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.ProcessIdIsEmpty);
    }
}
