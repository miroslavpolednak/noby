using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.DeleteRealEstateValuationAttachment;

internal sealed class DeleteRealEstateValuationAttachmentHandler
    : IRequestHandler<DeleteRealEstateValuationAttachmentRequest>
{
    public async Task Handle(DeleteRealEstateValuationAttachmentRequest request, CancellationToken cancellationToken)
    {
        await _realEstateValuationService.DeleteRealEstateValuationAttachment(request.RealEstateValuationAttachmentId, request.RealEstateValuationId, cancellationToken);
    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public DeleteRealEstateValuationAttachmentHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
