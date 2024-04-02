﻿using DomainServices.OfferService.Contracts;
using FluentValidation;

namespace DomainServices.OfferService.Api.Endpoints.v1.SimulateMortgageExtraPayment;

internal sealed class SimulateMortgageExtraPaymentRequestValidator
    : AbstractValidator<SimulateMortgageExtraPaymentRequest>
{
    public SimulateMortgageExtraPaymentRequestValidator()
    {

    }
}