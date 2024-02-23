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

        RuleFor(t => t.OrderId)
            .LessThanOrEqualTo(999999999)
            .WithErrorCode(90032);

        RuleFor(t => t.ProcessId)
            .NotEmpty()
            .WithErrorCode(90046);
    }

    private static int[] _allowedTaskTypes = 
        [
            (int)WorkflowTaskTypes.PriceException,
            (int)WorkflowTaskTypes.Consultation,
            (int)WorkflowTaskTypes.PredaniNaSpecialitu
        ];
}
