using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.UpdateActiveTasks;

internal class UpdateActiveTasksRequestValidator : AbstractValidator<UpdateActiveTasksRequest>
{
    public UpdateActiveTasksRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("Case Id must be > 0").WithErrorCode("13016");

        RuleFor(t => t.Tasks)
          .Must(tasks => !tasks.GroupBy(t => t.TaskProcessId).Any(i => i.Count() > 1))
          .WithMessage($"TaskProcessId must be unique").WithErrorCode("13001");
    }
}