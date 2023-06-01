using FluentValidation;

namespace NOBY.Api.Endpoints.Cases.CreateTask;

internal sealed class CreateTaskRequestValidator
    : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(t => t.TaskTypeId)
            .Must(t => _allowedTaskTypes.Contains(t))
            .WithMessage("Task type not allowed");

        RuleFor(t => t.SalesArrangementId)
            .NotEmpty()
            .WithMessage("SalesArrangementId is mandatory for TaskTypeId=2")
            .When(t => t.TaskTypeId == 2);
    }

    private static int[] _allowedTaskTypes = new[] { 2, 3, 7 };
}
