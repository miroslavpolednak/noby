namespace NOBY.Api.Endpoints.DocumentOnSA.SignDocumentManually;

public class SignDocumentManuallyRequest : IRequest
{
    public SignDocumentManuallyRequest(int salesArrangementId, int documentOnSAId)
    {
        SalesArrangementId = salesArrangementId;
        DocumentOnSAId = documentOnSAId;
    }

    public int SalesArrangementId { get; }
    
    public int DocumentOnSAId { get; }
}
