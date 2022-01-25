using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class GetCaseDetailMediatrRequestValidator : AbstractValidator<Dto.GetCaseDetailMediatrRequest>
{
    public GetCaseDetailMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("Case Id must be > 0").WithErrorCode("13016");
    }
}
