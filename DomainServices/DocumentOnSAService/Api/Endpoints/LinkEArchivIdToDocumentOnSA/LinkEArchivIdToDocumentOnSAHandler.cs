using CIS.Foms.Enums;
using DomainServices.DocumentOnSAService.Api.Common;
using DomainServices.DocumentOnSAService.Api.Database;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using FastEnumUtility;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.LinkEArchivIdToDocumentOnSA;

public class LinkEArchivIdToDocumentOnSAHandler : IRequestHandler<LinkEArchivIdToDocumentOnSARequest, Empty>
{
    private readonly DocumentOnSAServiceDbContext _context;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ISalesArrangementStateManager _salesArrangementStateManager;

    public LinkEArchivIdToDocumentOnSAHandler(
        DocumentOnSAServiceDbContext context,
        ISalesArrangementServiceClient salesArrangementService,
        ISalesArrangementStateManager salesArrangementStateManager)
    {
        _context = context;
        _salesArrangementService = salesArrangementService;
        _salesArrangementStateManager = salesArrangementStateManager;
    }

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
        if (salesArrangement.State == SalesArrangementStates.InSigning.ToByte())
        {
            await _salesArrangementStateManager.SetSalesArrangementStateAccordingDocumentsOnSa(salesArrangement.SalesArrangementId, cancellationToken);
        }

        return new Empty();
    }
}
