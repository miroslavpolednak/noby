using FluentValidation;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.GetDeedOfOwnershipDocuments;

internal sealed class GetDeedOfOwnershipDocumentsRequestValidator
    : AbstractValidator<Contracts.GetDeedOfOwnershipDocumentsRequest>
{
    public GetDeedOfOwnershipDocumentsRequestValidator()
    {
        RuleFor(t => t.RealEstateValuationId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.RealEstateValuationIdEmpty);
    }
}
