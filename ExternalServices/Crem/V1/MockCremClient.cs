using ExternalServices.Crem.V1.Contracts;

namespace ExternalServices.Crem.V1;

internal sealed class MockCremClient
    : ICremClient
{
    public Task<ICollection<DeedOfOwnershipDocument>> GetDocuments(int? katuzId, int? deedOfOwnershipNumber, long? deedOfOwnershipId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseGetFlatsForAddressDTO> GetFlatsForAddress(long addressPointId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<DeedOfOwnershipLegalRelation>> GetLegalRelations(long documentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<DeedOfOwnershipOwnerDTO>> GetOwners(long documentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<DeedOfOwnershipRealEstate>> GetRealEstates(long documentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<long> RequestNewDocumentId(int? katuzId, int? deedOfOwnershipNumber, long? deedOfOwnershipId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
