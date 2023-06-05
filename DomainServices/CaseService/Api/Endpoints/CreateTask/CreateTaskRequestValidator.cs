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
            .Must(t => t is 2 or 3 or 7)
            .WithErrorCode(ErrorCodeMapper.TaskTypeIdNotAllowed);

        RuleFor(t => t.ProcessId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.ProcessIdIsEmpty);

        When(t => t.TaskTypeId == 2,
             () =>
             {
                 RuleFor(t => t.PriceException)
                     .NotNull()
                     .WithErrorCode(ErrorCodeMapper.TaskPriceExceptionIsEmpty);
             });
    }
}
