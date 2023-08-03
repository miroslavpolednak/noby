﻿using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.UpdateDeedOfOwnershipDocument;

internal sealed class UpdateDeedOfOwnershipDocumentRequestValidator
    : AbstractValidator<UpdateDeedOfOwnershipDocumentRequest>
{
    public UpdateDeedOfOwnershipDocumentRequestValidator()
    {
        RuleFor(t => t.DeedOfOwnershipDocumentId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.DeedOfOwnershipDocumentIdIsEmpty);
    }
}