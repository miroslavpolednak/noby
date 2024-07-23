namespace NOBY.ApiContracts;

public partial class DocumentOnSaSearchDocumentsOnSaRequest:IRequest<DocumentOnSaSearchDocumentsOnSaResponse>
{
    
    [JsonIgnore]
    public int SalesArrangementId { get; set; }

    public DocumentOnSaSearchDocumentsOnSaRequest InfuseSalesArrangementId(int salesArrangementId)
    {
        SalesArrangementId = salesArrangementId;
        return this;
    }
}
