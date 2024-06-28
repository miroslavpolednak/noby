using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.DeleteRealEstateValuationAttachment;

internal sealed class DeleteRealEstateValuationAttachmentHandler(IRealEstateValuationServiceClient _realEstateValuationService)
        : IRequestHandler<DeleteRealEstateValuationAttachmentRequest>
{
    public async Task Handle(DeleteRealEstateValuationAttachmentRequest request, CancellationToken cancellationToken)
    {
        await _realEstateValuationService.DeleteRealEstateValuationAttachment(request.RealEstateValuationAttachmentId, request.RealEstateValuationId, cancellationToken);
    }
}
