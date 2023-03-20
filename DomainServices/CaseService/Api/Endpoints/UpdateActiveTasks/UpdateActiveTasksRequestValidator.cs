using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.UpdateActiveTasks;

internal sealed class UpdateActiveTasksRequestValidator 
    : AbstractValidator<UpdateActiveTasksRequest>
{
    public UpdateActiveTasksRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);

        RuleFor(t => t.Tasks)
          .Must(tasks => !tasks.GroupBy(t => t.TaskProcessId).Any(i => i.Count() > 1))
          .WithErrorCode(ErrorCodeMapper.TaskProcessIdNotUnique);
    }
}