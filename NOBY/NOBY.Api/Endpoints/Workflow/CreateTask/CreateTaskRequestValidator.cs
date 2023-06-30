using FluentValidation;

namespace NOBY.Api.Endpoints.Workflow.CreateTask;

internal sealed class CreateTaskRequestValidator
    : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(t => t.TaskTypeId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(t => _allowedTaskTypes.Contains(t))
            .WithMessage("Task type not allowed");

        RuleFor(t => t.ProcessId)
            .NotEmpty();
    }

    private static int[] _allowedTaskTypes = new[] { 2, 3, 7 };
}
