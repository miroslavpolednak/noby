using cz.mpss.api.epodpisy.digitalsigning.documentsigningevents.v1;
using DomainServices.DocumentOnSAService.Clients;
using KafkaFlow;

namespace NOBY.Api.Messaging.DocumentOnSA.V1;

public class DocumentStateChangedHandler(ILogger<DocumentStateChangedHandler> logger, IDocumentOnSAServiceClient documentOnSAService) : IMessageHandler<DocumentStateChanged>
{
    private readonly ILogger<DocumentStateChangedHandler> _logger = logger;
    private readonly IDocumentOnSAServiceClient _documentOnSAService = documentOnSAService;

    public async Task Handle(IMessageContext context, DocumentStateChanged message)
    {
        if (string.IsNullOrWhiteSpace(message.documentExternalId))
        {
            //ToDo log
        }


    }
}
