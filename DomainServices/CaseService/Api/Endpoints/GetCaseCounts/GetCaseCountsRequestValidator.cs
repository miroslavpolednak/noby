using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.GetCaseCounts;

internal sealed class GetCaseCountsRequestValidator
    : AbstractValidator<GetCaseCountsRequest>
{
    public GetCaseCountsRequestValidator()
    {
        RuleFor(t => t.CaseOwnerUserId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseOwnerIsEmpty);
    }
}
