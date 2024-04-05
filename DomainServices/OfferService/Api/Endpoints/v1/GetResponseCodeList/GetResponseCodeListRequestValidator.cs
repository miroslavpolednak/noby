using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.v1.GetResponseCodeList;

internal sealed class GetResponseCodeListRequestValidator
    : AbstractValidator<GetResponseCodeListRequest>
{
    public GetResponseCodeListRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);
    }
}
