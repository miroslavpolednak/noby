namespace FOMS.DocumentProcessing;

public sealed class DocumentProcessorFactory : IDocumentProcessorFactory
{
    public async Task<IDocumentProcessor> CreateDocumentProcessor(int salesArrangementId)
    {
        var salesArrangement = await _salesArrangementService.GetSalesArrangementDetail(salesArrangementId);

        return new HousingSavingsProcessor();
    }

    private readonly DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;

    public DocumentProcessorFactory(DomainServices.SalesArrangementService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _salesArrangementService = salesArrangementService;
    }
}
