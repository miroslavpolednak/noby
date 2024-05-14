using cz.mpss.api.epodpisy.digitalsigning.documentsigningevents.v1;
using DomainServices.DocumentOnSAService.Clients;
using KafkaFlow;

namespace NOBY.Api.Messaging.DocumentOnSA.V1;

public class DocumentStateChangedHandler(IDocumentOnSAServiceClient documentOnSAService) : IMessageHandler<DocumentStateChanged>
{
    private readonly IDocumentOnSAServiceClient _documentOnSAService = documentOnSAService;

    public async Task Handle(IMessageContext context, DocumentStateChanged message)
    {
        switch (message.state)
        {
            case DocumentStateEnum.SIGNED or DocumentStateEnum.VERIFIED or DocumentStateEnum.SENT:
                await _documentOnSAService.RefreshElectronicDocExternalId(new() { ExternalIdESignatures = message.documentExternalId, Operation = DomainServices.DocumentOnSAService.Contracts.Operation.SignDocument });
                break;
            case DocumentStateEnum.DELETED:
                await _documentOnSAService.RefreshElectronicDocExternalId(new() { ExternalIdESignatures = message.documentExternalId, Operation = DomainServices.DocumentOnSAService.Contracts.Operation.StopSigning });
                break;
            case DocumentStateEnum.NEW or DocumentStateEnum.IN_PROGRESS or DocumentStateEnum.APPROVED:
                // Ignore
                break;
            default:
                throw new ArgumentException(nameof(message.state));
        }
    }
}
