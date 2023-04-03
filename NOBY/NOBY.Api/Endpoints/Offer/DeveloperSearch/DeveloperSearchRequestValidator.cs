﻿using CIS.Infrastructure.WebApi.Validation;
using FluentValidation;

namespace NOBY.Api.Endpoints.Offer.DeveloperSearch;

internal sealed class DeveloperSearchRequestValidator
    : AbstractValidator<DeveloperSearchRequest>
{
    public DeveloperSearchRequestValidator()
    {
        RuleFor(t => t.SearchText)
            .NotEmpty();

        RuleFor(t => t.Pagination).SetValidator(new PaginationRequestValidator());
    }
}