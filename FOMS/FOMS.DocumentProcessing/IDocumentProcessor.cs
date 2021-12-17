namespace FOMS.DocumentProcessing;

public interface IDocumentProcessor
{
    Task<object> GetPart(int partId);

    Task SavePart(int partId, object data);
}
