using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class GetCaseListMediatrRequest : AbstractValidator<Dto.GetCaseListMediatrRequest>
{
    public GetCaseListMediatrRequest()
    {
        RuleFor(t => t.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be > 0").WithErrorCode("13000");
        
        // strankovani asi neni povinne?
    }
}
