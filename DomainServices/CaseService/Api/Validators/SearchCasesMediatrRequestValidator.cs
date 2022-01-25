using FluentValidation;

namespace DomainServices.CaseService.Api.Validators;

internal class SearchCasesMediatrRequest : AbstractValidator<Dto.SearchCasesMediatrRequest>
{
    public SearchCasesMediatrRequest()
    {
        RuleFor(t => t.CaseOwnerUserId)
            .GreaterThan(0)
            .WithMessage("CaseOwnerUserId must be > 0").WithErrorCode("13003");
    }
}
