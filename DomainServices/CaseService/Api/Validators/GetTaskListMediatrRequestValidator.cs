using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class GetTaskListMediatrRequestValidator : AbstractValidator<Dto.GetTaskListMediatrRequest>
{
    public GetTaskListMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("Case Id must be > 0").WithErrorCode("13016");
    }
}