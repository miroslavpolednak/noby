using DomainServices.RealEstateValuationService.Contracts;
using StackExchange.Redis;

namespace DomainServices.RealEstateValuationService.Clients.Services;

internal sealed class RealEstateValuationServiceClient
    : IRealEstateValuationServiceClient
{
    public async Task<int> CreateRealEstateValuation(CreateRealEstateValuationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateRealEstateValuationAsync(request, cancellationToken: cancellationToken);
        return result.RealEstateValuationId;
    }

    public async Task DeleteRealEstateValuation(long caseId, int realEstateValuationId, CancellationToken cancellationToken = default)
    {
        await _service.DeleteRealEstateValuationAsync(new()
        {
            CaseId = caseId,
            RealEstateValuationId = realEstateValuationId
        }, cancellationToken: cancellationToken);
    }

    public async Task<List<RealEstateValuationListItem>> GetRealEstateValuationList(long caseId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetRealEstateValuationListAsync(new()
        {
            CaseId = caseId
        }, cancellationToken: cancellationToken);
        return result.RealEstateValuationList.ToList();
    }

    public async Task PatchDeveloperOnRealEstateValuation(int realEstateValuationId, int valuationStateId, bool developerApplied, CancellationToken cancellationToken = default)
    {
        await _service.PatchDeveloperOnRealEstateValuationAsync(new()
        {
            RealEstateValuationId = realEstateValuationId,
            ValuationStateId = valuationStateId,
            DeveloperApplied = developerApplied
        }, cancellationToken: cancellationToken);
    }

    public async Task<RealEstateValuationDetail> GetRealEstateValuationDetail(int realEstateValuationId, CancellationToken cancellationToken = default)
    {
        return await _service.GetRealEstateValuationDetailAsync(new()
        {
            RealEstateValuationId = realEstateValuationId
        }, cancellationToken: cancellationToken);
    }

    public async Task<RealEstateValuationDetail> GetRealEstateValuationDetailByOrderId(int orderId, CancellationToken cancellationToken = default)
    {
        return await _service.GetRealEstateValuationDetailByOrderIdAsync(new GetRealEstateValuationDetailByOrderIdRequest
        {
            OrderId = orderId
        }, cancellationToken: cancellationToken);
    }

    public async Task UpdateRealEstateValuationDetail(UpdateRealEstateValuationDetailRequest request, CancellationToken cancellationToken = default)
    {
        await _service.UpdateRealEstateValuationDetailAsync(request, cancellationToken: cancellationToken);
    }

    public async Task SetACVRealEstateTypeByRealEstateValuation(int realEstateValuationId, string ACVRealEstateTypeId, CancellationToken cancellationToken = default)
    {
        await _service.SetACVRealEstateTypeByRealEstateValuationAsync(new()
        {
            RealEstateValuationId = realEstateValuationId,
            ACVRealEstateType = ACVRealEstateTypeId
        }, cancellationToken: cancellationToken);
    }

    public async Task UpdateStateByRealEstateValuation(int realEstateValuationId, int valuationStateId, CancellationToken cancellationToken = default)
    {
        await _service.UpdateStateByRealEstateValuationAsync(new()
        {
            RealEstateValuationId = realEstateValuationId,
            ValuationStateId = valuationStateId
        }, cancellationToken: cancellationToken);
    }

    public async Task<int> CreateRealEstateValuationAttachment(CreateRealEstateValuationAttachmentRequest request, CancellationToken cancellationToken = default)
    {
        return (await _service.CreateRealEstateValuationAttachmentAsync(request, cancellationToken: cancellationToken)).RealEstateValuationAttachmentId;
    }

    public async Task DeleteRealEstateValuationAttachment(int realEstateValuationAttachmentId, int realEstateValuationId, CancellationToken cancellationToken = default)
    {
        await _service.DeleteRealEstateValuationAttachmentAsync(new()
        {
            RealEstateValuationId = realEstateValuationId,
            RealEstateValuationAttachmentId = realEstateValuationAttachmentId
        }, cancellationToken: cancellationToken);
    }

    public async Task<ValidateRealEstateValuationIdResponse> ValidateRealEstateValuationId(int realEstateValuationId, bool throwExceptionIfNotFound = false, CancellationToken cancellationToken = default)
    {
        return await _service.ValidateRealEstateValuationIdAsync(new()
        {
            ThrowExceptionIfNotFound = throwExceptionIfNotFound,
            RealEstateValuationId = realEstateValuationId
        }, cancellationToken: cancellationToken);
    }

    // DeedOfOwnershipDocument
    public async Task<int> AddDeedOfOwnershipDocument(AddDeedOfOwnershipDocumentRequest request, CancellationToken cancellationToken = default)
    {
        return (await _service.AddDeedOfOwnershipDocumentAsync(request, cancellationToken: cancellationToken)).DeedOfOwnershipDocumentId;
    }

    public async Task<Contracts.DeedOfOwnershipDocument> GetDeedOfOwnershipDocument(int deedOfOwnershipDocumentId, CancellationToken cancellationToken = default)
    {
        return await _service.GetDeedOfOwnershipDocumentAsync(new()
        {
            DeedOfOwnershipDocumentId = deedOfOwnershipDocumentId
        }, cancellationToken: cancellationToken);
    }

    public async Task DeleteDeedOfOwnershipDocument(int deedOfOwnershipDocumentId, CancellationToken cancellationToken = default)
    {
        await _service.DeleteDeedOfOwnershipDocumentAsync(new()
        {
            DeedOfOwnershipDocumentId = deedOfOwnershipDocumentId
        }, cancellationToken: cancellationToken);
    }

    public async Task<List<DeedOfOwnershipDocument>> GetDeedOfOwnershipDocuments(int realEstateValuationId, CancellationToken cancellationToken = default)
    {
        return (await _service.GetDeedOfOwnershipDocumentsAsync(new()
        {
            RealEstateValuationId = realEstateValuationId,
        }, cancellationToken: cancellationToken))
        .Documents
        .ToList();
    }

    private readonly Contracts.v1.RealEstateValuationService.RealEstateValuationServiceClient _service;
    public RealEstateValuationServiceClient(Contracts.v1.RealEstateValuationService.RealEstateValuationServiceClient service)
        => _service = service;
}
