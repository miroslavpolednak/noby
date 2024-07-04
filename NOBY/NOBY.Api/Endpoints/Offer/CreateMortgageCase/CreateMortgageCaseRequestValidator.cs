﻿using FluentValidation;

namespace NOBY.Api.Endpoints.Offer.CreateMortgageCase;

internal sealed class CreateMortgageCaseRequestValidator
    : AbstractValidator<OfferCreateMortgageCaseRequest>
{
    public CreateMortgageCaseRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithMessage("OfferId is not set");

        RuleFor(t => t.FirstName)
            .NotEmpty()
            .When(t => t.Identity is null)
            .WithMessage("Jméno není vyplněné");

        RuleFor(t => t.LastName)
            .NotEmpty()
            .When(t => t.Identity is null)
            .WithMessage("Příjmení není vyplněné");
    }
}
