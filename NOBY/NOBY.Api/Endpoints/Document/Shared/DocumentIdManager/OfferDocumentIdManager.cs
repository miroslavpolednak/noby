using CIS.Core.Attributes;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Document.Shared.DocumentIdManager;

[TransientService, SelfService]
internal class OfferDocumentIdManager : IDocumentIdManager<int>
{
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public OfferDocumentIdManager(ISalesArrangementServiceClient salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }

    public async Task<string> LoadDocumentId(int salesArrangementId, CancellationToken cancellationToken)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);

        return salesArrangement.OfferDocumentId;
    }

    public Task UpdateDocumentId(int salesArrangementId, string documentId, CancellationToken cancellationToken)
    {
        return _salesArrangementService.UpdateOfferDocumentId(salesArrangementId, documentId, cancellationToken);
    }
}