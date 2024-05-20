using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using KafkaFlow;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.DocumentOnSAService.Api.Messaging.DocumentStateChanged;

public sealed class DocumentStateChangedHandler(
	DocumentOnSAServiceDbContext _context, 
	IMediator _mediator) 
    : IMessageHandler<DocumentStateChanged>
{
    public async Task Handle(IMessageContext context, DocumentStateChanged message)
    {
        switch (message.state)
        {
            case DocumentStateEnum.SIGNED or DocumentStateEnum.VERIFIED or DocumentStateEnum.SENT:
				await signDocument(message.documentExternalId, Operations.SignDocument);
                break;
            case DocumentStateEnum.DELETED:
				await signDocument(message.documentExternalId, Operations.StopSigning);
                break;
        }
	}

	private async Task signDocument(string externalIdESignatures, Operations operation)
	{
		var docOnSa = await _context.DocumentOnSa
			.Select(s => new
			{
				s.DocumentOnSAId,
				s.ExternalIdESignatures,
				s.SignatureTypeId,
				s.IsValid,
				s.IsSigned
			})
			.FirstOrDefaultAsync(d => d.ExternalIdESignatures == externalIdESignatures);

		if (docOnSa is not null && operation == Operations.SignDocument && docOnSa is { IsValid: true, IsSigned: false })
		{
			await _mediator.Send(new SignDocumentRequest { DocumentOnSAId = docOnSa.DocumentOnSAId, SignatureTypeId = docOnSa.SignatureTypeId });
		}
		else if (docOnSa is not null && operation == Operations.StopSigning)
		{
			await _mediator.Send(new StopSigningRequest { DocumentOnSAId = docOnSa.DocumentOnSAId, NotifyESignatures = false });
		}
	}

	enum Operations
	{
		SignDocument, 
		StopSigning
	}
}
