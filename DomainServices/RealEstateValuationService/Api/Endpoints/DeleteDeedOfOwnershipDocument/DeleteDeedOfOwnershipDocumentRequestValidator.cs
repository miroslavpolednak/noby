using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.DeleteDeedOfOwnershipDocument;

internal sealed class DeleteDeedOfOwnershipDocumentRequestValidator
    : AbstractValidator<DeleteDeedOfOwnershipDocumentRequest>
{
    public DeleteDeedOfOwnershipDocumentRequestValidator()
    {
        RuleFor(t => t.DeedOfOwnershipDocumentId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.DeedOfOwnershipDocumentIdIsEmpty);
    }
}
