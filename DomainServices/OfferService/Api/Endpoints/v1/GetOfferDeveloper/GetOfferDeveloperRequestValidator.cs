﻿using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.v1.GetOfferDeveloper;

internal sealed class GetOfferDeveloperRequestValidator
    : AbstractValidator<GetOfferDeveloperRequest>
{
    public GetOfferDeveloperRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.OfferIdIsEmpty);
    }
}
