namespace FOMS.DocumentProcessing;

internal class HousingSavingsProcessor : BaseDocumentProcessor, IDocumentProcessor
{
    public HousingSavingsProcessor(ServiceAccessor serviceAccessor, DomainServices.SalesArrangementService.Contracts.GetSalesArrangementResponse salesArrangement)
        : base(serviceAccessor, salesArrangement) 
    { }

    private DocumentContracts.HousingSavings.HousingSavingsContract getNewContract()
        => new DocumentContracts.HousingSavings.HousingSavingsContract();

    public async Task<object> GetPart(int partId)
    {
        var saObject = await getSalesArrangementData<DocumentContracts.HousingSavings.HousingSavingsContract>() ?? getNewContract();
        
        return saObject.GetPart(partId);
    }

    public async Task SavePart(int partId, object data)
    {
        var saObject = await getSalesArrangementData<DocumentContracts.HousingSavings.HousingSavingsContract>() ?? getNewContract();

        saObject.MergePart(partId, data);

        saveSalesArrangementData(saObject);
    }
}
