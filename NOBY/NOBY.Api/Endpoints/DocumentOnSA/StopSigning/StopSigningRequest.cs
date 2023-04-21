namespace NOBY.Api.Endpoints.DocumentOnSA.StopSigning;

public class StopSigningRequest : IRequest
{
	public StopSigningRequest(int salesArrangementId,int documentOnSAId)
	{
        SalesArrangementId = salesArrangementId;
        DocumentOnSAId = documentOnSAId;
    }

    public int SalesArrangementId { get; }

    public int DocumentOnSAId { get; }
}
