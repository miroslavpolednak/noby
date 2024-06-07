using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using KafkaFlow;
using Microsoft.EntityFrameworkCore;
using E = cz.mpss.api.epodpisy.digitalsigning.documentsigningevents.v1;

namespace DomainServices.DocumentOnSAService.Api.Messaging.DocumentStateChanged;

public sealed class DocumentStateChangedHandler(
	DocumentOnSAServiceDbContext _context, 
	IMediator _mediator) 
    : IMessageHandler<E.DocumentStateChanged>
{
    public async Task Handle(IMessageContext context, E.DocumentStateChanged message)
    {
        switch (message.state)
        {
            case E.DocumentStateEnum.SIGNED or E.DocumentStateEnum.VERIFIED or E.DocumentStateEnum.SENT:
				await signDocument(message.documentExternalId, Operations.SignDocument);
                break;
            case E.DocumentStateEnum.DELETED:
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
