using DomainServices.RealEstateValuationService.Contracts;

namespace DomainServices.RealEstateValuationService.Clients;

public interface IRealEstateValuationServiceClient
{
    Task<int> CreateRealEstateValuation(CreateRealEstateValuationRequest request, CancellationToken cancellationToken = default);

    Task DeleteRealEstateValuation(long caseId, int realEstateValuationId, CancellationToken cancellationToken = default);

    Task<List<RealEstateValuationListItem>> GetRealEstateValuationList(long caseId, CancellationToken cancellationToken = default);

    Task PatchDeveloperOnRealEstateValuation(int realEstateValuationId, int valuationStateId, bool developerApplied, CancellationToken cancellationToken = default);

    Task<RealEstateValuationDetail> GetRealEstateValuationDetail(int realEstateValuationId, CancellationToken cancellationToken = default);

    Task<RealEstateValuationDetail> GetRealEstateValuationDetailByOrderId(int orderId, CancellationToken cancellationToken = default);

    Task UpdateRealEstateValuationDetail(UpdateRealEstateValuationDetailRequest request, CancellationToken cancellationToken = default);

    Task SetACVRealEstateTypeByRealEstateValuation(int realEstateValuationId, string ACVRealEstateTypeId, CancellationToken cancellationToken = default);

    Task UpdateStateByRealEstateValuation(int realEstateValuationId, int valuationStateId, CancellationToken cancellationToken = default);

    Task<int> AddDeedOfOwnershipDocument(AddDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken = default);

    Task DeleteDeedOfOwnershipDocument(int deedOfOwnershipDocumentId, CancellationToken cancellationToken = default);

    Task<int> CreateRealEstateValuationAttachment(CreateRealEstateValuationAttachmentRequest request, CancellationToken cancellationToken = default);

    Task DeleteRealEstateValuationAttachment(int realEstateValuationAttachmentId, CancellationToken cancellationToken = default);
}
