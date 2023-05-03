using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.CompleteTask;

internal class CompleteTaskRequestValidator : AbstractValidator<CompleteTaskRequest>
{
    public CompleteTaskRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);

        RuleFor(t => t.TaskIdSb)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.TaskIdSBIsEmpty);

        RuleFor(t => t.TaskTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.TaskTypeIdIsEmpty);
    }
}