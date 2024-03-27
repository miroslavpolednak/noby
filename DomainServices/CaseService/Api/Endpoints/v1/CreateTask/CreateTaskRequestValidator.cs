using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.v1.CreateTask;

internal sealed class CreateTaskRequestValidator
    : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(t => t.TaskTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.TaskTypeIdIsEmpty)
            .Must(t => t is 2 or 3 or 7 or 9)
            .WithErrorCode(ErrorCodeMapper.TaskTypeIdNotAllowed);

        When(t => t.TaskTypeId is 2 or 3 or 7, () =>
        {
            RuleFor(t => t.ProcessId)
            .NotNull()
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.ProcessIdIsEmpty);
        });

        When(t => t.TaskTypeId == 2,
             () =>
             {
                 RuleFor(t => t.PriceException)
                     .NotNull()
                     .WithErrorCode(ErrorCodeMapper.TaskPriceExceptionIsEmpty);
             });
    }
}
