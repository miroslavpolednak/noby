namespace FOMS.DocumentProcessing;

public interface IDocumentProcessorFactory
{
    IDocumentProcessor CreateDocumentProcessor(int salesArrangementType, int salesArrangementId);
}
