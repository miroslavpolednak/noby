﻿using FluentValidation;

namespace FOMS.Api.Endpoints.Offer.GetMortgageBySalesArrangement;

internal class GetMortgageBySalesArrangementRequestValidator
    : AbstractValidator<GetMortgageBySalesArrangementRequest>
{
    public GetMortgageBySalesArrangementRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0");
    }
}