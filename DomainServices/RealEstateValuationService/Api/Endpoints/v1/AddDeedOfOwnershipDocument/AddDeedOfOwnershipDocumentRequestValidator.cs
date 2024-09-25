using DomainServices.RealEstateValuationService.Contracts;
using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.AddDeedOfOwnershipDocument;

internal sealed class AddDeedOfOwnershipDocumentRequestValidator
    : AbstractValidator<AddDeedOfOwnershipDocumentRequest>
{
    public AddDeedOfOwnershipDocumentRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);
    }
}
