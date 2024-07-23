using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.v1.SearchCases;

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
