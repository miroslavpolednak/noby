using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.GetDeedOfOwnershipDocument;

internal sealed class GetDeedOfOwnershipDocumentRequestValidator
    : AbstractValidator<Contracts.GetDeedOfOwnershipDocumentRequest>
{
    public GetDeedOfOwnershipDocumentRequestValidator()
    {
        RuleFor(t => t.DeedOfOwnershipDocumentId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.DeedOfOwnershipDocumentIdIsEmpty);
    }
}
