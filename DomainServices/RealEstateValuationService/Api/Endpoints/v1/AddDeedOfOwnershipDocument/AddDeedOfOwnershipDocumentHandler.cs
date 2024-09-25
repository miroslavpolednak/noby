using DomainServices.RealEstateValuationService.Api.Database;
using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Api.Endpoints.v1.AddDeedOfOwnershipDocument;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method
internal sealed class AddDeedOfOwnershipDocumentHandler(RealEstateValuationServiceDbContext _dbContext)
        : IRequestHandler<AddDeedOfOwnershipDocumentRequest, AddDeedOfOwnershipDocumentResponse>
{
    public async Task<AddDeedOfOwnershipDocumentResponse> Handle(AddDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken)
    {
        if (!await _dbContext
            .RealEstateValuations
            .AnyAsync(t => t.RealEstateValuationId == request.RealEstateValuationId, cancellationToken))
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.RealEstateValuationNotFound, request.RealEstateValuationId);
        }

        var entity = new Database.Entities.DeedOfOwnershipDocument
        {
            RealEstateValuationId = request.RealEstateValuationId,
            DeedOfOwnershipId = request.DeedOfOwnershipId,
            Address = request.Address,
            DeedOfOwnershipNumber = request.DeedOfOwnershipNumber,
            CremDeedOfOwnershipDocumentId = request.CremDeedOfOwnershipDocumentId,
            KatuzId = request.KatuzId,
            KatuzTitle = request.KatuzTitle,
            AddressPointId = request.AddressPointId,
            RealEstateIds = request.RealEstateIds?.Any() ?? false ? request.RealEstateIds.ToList() : null
        };
        _dbContext.DeedOfOwnershipDocuments.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AddDeedOfOwnershipDocumentResponse
        {
            DeedOfOwnershipDocumentId = entity.DeedOfOwnershipDocumentId
        };
    }
}
