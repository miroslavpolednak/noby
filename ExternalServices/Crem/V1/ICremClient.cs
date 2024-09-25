using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.Crem.V1.Contracts;

namespace ExternalServices.Crem.V1;

public interface ICremClient
    : IExternalServiceClient
{
    Task<(long CremDeedOfOwnershipDocumentId, int DeedOfOwnershipNumber)> RequestNewDocumentId(int? katuzId, int? deedOfOwnershipNumber, long? deedOfOwnershipId, CancellationToken cancellationToken = default);

    Task<ResponseGetFlatsForAddressDTO> GetFlatsForAddress(long addressPointId, CancellationToken cancellationToken = default);

    Task<ICollection<DeedOfOwnershipDocument>> GetDocuments(int? katuzId, int? deedOfOwnershipNumber, long? deedOfOwnershipId, CancellationToken cancellationToken = default);

    Task<ICollection<DeedOfOwnershipOwnerDTO>> GetOwners(long documentId, CancellationToken cancellationToken = default);

    Task<ICollection<DeedOfOwnershipLegalRelation>> GetLegalRelations(long documentId, CancellationToken cancellationToken = default);

    Task<ICollection<DeedOfOwnershipRealEstate>> GetRealEstates(long documentId, CancellationToken cancellationToken = default);

    Task<bool> TryToConfirmDownload(long documentId, CancellationToken cancellationToken);

    const string Version = "V1";
}
