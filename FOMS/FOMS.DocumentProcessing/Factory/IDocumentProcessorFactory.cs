namespace FOMS.DocumentProcessing;

public interface IDocumentProcessorFactory
{
    Task<IDocumentProcessor> CreateDocumentProcessor(int salesArrangementId);
}
