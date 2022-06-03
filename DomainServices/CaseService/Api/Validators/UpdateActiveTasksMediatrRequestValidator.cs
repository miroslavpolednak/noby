using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class UpdateActiveTasksMediatrRequestValidator : AbstractValidator<Dto.UpdateActiveTasksMediatrRequest>
{
    public UpdateActiveTasksMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("Case Id must be > 0").WithErrorCode("13016");

        RuleFor(t => t.Tasks)
          .Must(tasks => !tasks.GroupBy(t => t.TaskId).Any(i => i.Count() > 1))     
          .WithMessage($"TaskId must be unique").WithErrorCode("99999"); //TODO: ErrorCode
    }
}