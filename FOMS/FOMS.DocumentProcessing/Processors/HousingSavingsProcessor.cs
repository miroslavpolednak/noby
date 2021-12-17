namespace FOMS.DocumentProcessing;

internal class HousingSavingsProcessor : BaseDocumentProcessor, IDocumentProcessor
{
    public HousingSavingsProcessor(ServiceAccessor serviceAccessor, int salesArrangementId) 
        : base(serviceAccessor, salesArrangementId) 
    { }

    public async Task<object> GetPart(int partId)
    {
        return null;
    }

    public async Task SavePart(int partId, object data)
    {

    }
}
