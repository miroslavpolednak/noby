namespace NOBY.Api.Endpoints.RealEstateValuation.DeleteRealEstateValuationAttachment;

internal sealed record DeleteRealEstateValuationAttachmentRequest(long CaseId, int RealEstateValuationId, int RealEstateValuationAttachmentId)
    : IRequest
{
}
