using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.Crem.V1;

public interface ICremClient
    : IExternalServiceClient
{
    Task<long> RequestNewDocumentId(int? katuzId, int? deedOfOwnershipNumber, long? deedOfOwnershipId, CancellationToken cancellationToken = default);

    Task<Contracts.ResponseGetFlatsForAddressDTO> GetFlatsForAddress(long addressPointId, CancellationToken cancellationToken = default);

    Task<ICollection<Contracts.DeedOfOwnershipDocument>> GetDocuments(int? katuzId, int? deedOfOwnershipNumber, long? deedOfOwnershipId, CancellationToken cancellationToken = default);

    Task<ICollection<Contracts.DeedOfOwnershipOwnerDTO>> GetOwners(long documentId, CancellationToken cancellationToken = default);

    Task<ICollection<Contracts.DeedOfOwnershipLegalRelation>> GetLegalRelations(long documentId, CancellationToken cancellationToken = default);

    Task<ICollection<Contracts.DeedOfOwnershipRealEstate>> GetRealEstates(long documentId, CancellationToken cancellationToken = default);

    const string Version = "V1";
}
