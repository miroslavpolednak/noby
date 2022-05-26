using FluentValidation;

namespace DomainServices.OfferService.Api.Validators;

internal class GetMortgageOfferRequestValidator : AbstractValidator<Dto.GetMortgageOfferMediatrRequest>
{
    public GetMortgageOfferRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId is not specified").WithErrorCode("10005"); //TODO: ErrorCode
    }
}