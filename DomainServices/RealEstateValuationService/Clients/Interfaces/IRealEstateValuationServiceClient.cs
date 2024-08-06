using DomainServices.RealEstateValuationService.Contracts;
using SharedTypes.Enums;

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

    Task SetForeignRealEstateTypesByRealEstateValuation(int realEstateValuationId, string ACVRealEstateTypeId, string bagmanRealEstateTypeId, CancellationToken cancellationToken = default);

    Task UpdateStateByRealEstateValuation(int realEstateValuationId, RealEstateValuationStates valuationStateId, CancellationToken cancellationToken = default);

    Task UpdateValuationTypeByRealEstateValuation(int realEstateValuationId, int valuationTypeId, CancellationToken cancellationToken = default);

    Task<int> CreateRealEstateValuationAttachment(CreateRealEstateValuationAttachmentRequest request, CancellationToken cancellationToken = default);

    Task DeleteRealEstateValuationAttachment(int realEstateValuationAttachmentId, int realEstateValuationId, CancellationToken cancellationToken = default);

    Task<ValidateRealEstateValuationIdResponse> ValidateRealEstateValuationId(int realEstateValuationId, bool throwExceptionIfNotFound = false, CancellationToken cancellationToken = default);

    Task<List<SharedTypes.Enums.EnumRealEstateValuationTypes>> GetRealEstateValuationTypes(GetRealEstateValuationTypesRequest request, CancellationToken cancellationToken = default);

    Task PreorderOnlineValuation(PreorderOnlineValuationRequest request, CancellationToken cancellationToken = default);

    Task OrderOnlineValuation(OrderOnlineValuationRequest request, CancellationToken cancellationToken = default);

    Task OrderStandardValuation(OrderStandardValuationRequest request, CancellationToken cancellationToken = default);

    Task OrderDTSValuation(int realEstateValuationId, CancellationToken cancellationToken = default);

    // DeedOfOwnershipDocument
    Task UpdateDeedOfOwnershipDocument(int deedOfOwnershipDocumentId, List<long>? realEstateIds, CancellationToken cancellationToken = default);

    Task<int> AddDeedOfOwnershipDocument(AddDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken = default);

    Task DeleteDeedOfOwnershipDocument(int deedOfOwnershipDocumentId, CancellationToken cancellationToken = default);

    Task<List<Contracts.DeedOfOwnershipDocument>> GetDeedOfOwnershipDocuments(int realEstateValuationId, CancellationToken cancellationToken = default);

    Task<Contracts.DeedOfOwnershipDocument> GetDeedOfOwnershipDocument(int deedOfOwnershipDocumentId, CancellationToken cancellationToken = default);
}
