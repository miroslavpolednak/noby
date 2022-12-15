using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.SearchCases;

internal class SearchCasesMediatrRequest : AbstractValidator<SearchCasesRequest>
{
    public SearchCasesMediatrRequest()
    {
        RuleFor(t => t.CaseOwnerUserId)
            .GreaterThan(0)
            .WithMessage("CaseOwnerUserId must be > 0").WithErrorCode("13003");
    }
}
