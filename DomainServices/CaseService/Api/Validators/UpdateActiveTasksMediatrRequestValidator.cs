﻿using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class UpdateActiveTasksMediatrRequestValidator : AbstractValidator<Dto.UpdateActiveTasksMediatrRequest>
{
    public UpdateActiveTasksMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("Case Id must be > 0").WithErrorCode("13016");

        RuleFor(t => t.Tasks)
          .Must(tasks => !tasks.GroupBy(t => t.TaskProcessId).Any(i => i.Count() > 1))     
          .WithMessage($"TaskProcessId must be unique").WithErrorCode("13001");
    }
}