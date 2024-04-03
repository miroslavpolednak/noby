using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.v1.GetOfferList;

internal sealed class GetOfferListRequestValidator
    : AbstractValidator<GetOfferListRequest>
{
    public GetOfferListRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);
    }
}
