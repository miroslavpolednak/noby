using FluentValidation;

namespace DomainServices.OfferService.Api.Validators;

internal class GetOfferRequestValidator : AbstractValidator<Dto.GetOfferMediatrRequest>
{
    public GetOfferRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId is not specified").WithErrorCode("10001");
    }
}