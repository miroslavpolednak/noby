using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class GetCaseCountsMediatrRequestValidator 
    : AbstractValidator<Dto.GetCaseCountsMediatrRequest>
{
    public GetCaseCountsMediatrRequestValidator()
    {
        RuleFor(t => t.CaseOwnerUserId)
            .GreaterThan(0)
            .WithMessage("CaseOwnerUserId must be > 0").WithErrorCode("13000");
    }
}
