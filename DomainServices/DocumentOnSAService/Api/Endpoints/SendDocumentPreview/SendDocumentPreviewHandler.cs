using DomainServices.DocumentOnSAService.Contracts;
using ExternalServices.ESignatures.V1;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.SendDocumentPreview;

public class SendDocumentPreviewHandler : IRequestHandler<SendDocumentPreviewRequest, Empty>
{
    private readonly IESignaturesClient _signaturesClient;

    public async Task<Empty> Handle(SendDocumentPreviewRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        return new Empty();
    }

    public SendDocumentPreviewHandler(IESignaturesClient signaturesClient)
    {
        _signaturesClient = signaturesClient;
    }
}