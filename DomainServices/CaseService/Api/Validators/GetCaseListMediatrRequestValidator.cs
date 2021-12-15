using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class GetCaseListMediatrRequest : AbstractValidator<Dto.GetCaseListMediatrRequest>
{
    public GetCaseListMediatrRequest()
    {
        RuleFor(t => t.PartyId)
            .GreaterThan(0)
            .WithMessage("PartyId must be > 0").WithErrorCode("13000");
        
        // strankovani asi neni povinne?
    }
}
