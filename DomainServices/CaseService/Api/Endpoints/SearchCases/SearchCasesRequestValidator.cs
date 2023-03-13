using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.SearchCases;

internal sealed class SearchCasesMediatrRequest 
    : AbstractValidator<SearchCasesRequest>
{
    public SearchCasesMediatrRequest()
    {
        RuleFor(t => t.CaseOwnerUserId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseOwnerIsEmpty);
    }
}
