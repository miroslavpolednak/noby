﻿using FluentValidation;

namespace FOMS.Api.Endpoints.SalesArrangement.GetList;

internal class GetListRequestValidator
    : AbstractValidator<GetListRequest>
{
    public GetListRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0");
    }
}