using Microsoft.AspNetCore.Http;

namespace FOMS.DocumentProcessing;

public sealed class DocumentProcessorFactory : IDocumentProcessorFactory
{
    public IDocumentProcessor CreateDocumentProcessor(int salesArrangementType, int salesArrangementId)
    {
        // zatim switchem, az budu vedet jakym zpusobem se s SA pracuje, tak udelame nejakou automatiku
        switch (salesArrangementType)
        {
            case 1:
                return new HousingSavingsProcessor(_serviceAccessor, salesArrangementId);
            default:
                throw new NotImplementedException();
        }
        
    }

    private readonly ServiceAccessor _serviceAccessor;

    public DocumentProcessorFactory(HttpContextAccessor httpContext)
    {
        _serviceAccessor = new ServiceAccessor(httpContext);
    }
}
