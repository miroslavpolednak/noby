using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.DeleteRealEstateValuationAttachment;

internal sealed class DeleteRealEstateValuationAttachmentHandler
    : IRequestHandler<DeleteRealEstateValuationAttachmentRequest>
{
    public async Task Handle(DeleteRealEstateValuationAttachmentRequest request, CancellationToken cancellationToken)
    {
        var instance = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

        // podvrhnute caseId
        if (instance.RealEstateValuationGeneralDetails.CaseId != request.CaseId)
        {
            throw new CisAuthorizationException();
        }

        // spatny stav REV
        if (instance.RealEstateValuationGeneralDetails.ValuationStateId != 7)
        {
            throw new CisAuthorizationException();
        }

        // podvrhnute ID attachmentu
        if (!instance.Attachments.Any(t => t.RealEstateValuationAttachmentId == request.RealEstateValuationAttachmentId))
        {
            throw new CisAuthorizationException();
        }

        await _realEstateValuationService.DeleteRealEstateValuationAttachment(request.RealEstateValuationAttachmentId, cancellationToken);
    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public DeleteRealEstateValuationAttachmentHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
