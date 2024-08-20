using SharedTypes.Enums;
using DomainServices.DocumentOnSAService.Api.Common;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.LinkEArchivIdToDocumentOnSA;

public class LinkEArchivIdToDocumentOnSAHandler(
	DocumentOnSAServiceDbContext _context,
	ISalesArrangementServiceClient _salesArrangementService,
	ISalesArrangementStateManager _salesArrangementStateManager) 
    : IRequestHandler<LinkEArchivIdToDocumentOnSARequest, Empty>
{
	public async Task<Empty> Handle(LinkEArchivIdToDocumentOnSARequest request, CancellationToken cancellationToken)
    {
        var documentOnSaEntity = _context.DocumentOnSa.FirstOrDefault(r => r.DocumentOnSAId == request.DocumentOnSAId)
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.DocumentOnSANotExist);

        documentOnSaEntity.EArchivIdsLinkeds.Add(new()
        {
            EArchivId = request.EArchivId,
        });
        
        await _context.SaveChangesAsync(cancellationToken);

        var salesArrangement = await _salesArrangementService.GetSalesArrangement(documentOnSaEntity.SalesArrangementId, cancellationToken);
        // SA state
        if (salesArrangement.IsInState([EnumSalesArrangementStates.InSigning]))
        {
            await _salesArrangementStateManager.SetSalesArrangementStateAccordingDocumentsOnSa(salesArrangement.SalesArrangementId, cancellationToken);
        }

        return new Empty();
    }
}
