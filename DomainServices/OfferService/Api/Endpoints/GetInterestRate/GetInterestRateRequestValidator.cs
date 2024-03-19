using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.GetInterestRate;

internal sealed class GetInterestRateRequestValidator
    : AbstractValidator<GetInterestRateRequest>
{
    public GetInterestRateRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);
    }
}
