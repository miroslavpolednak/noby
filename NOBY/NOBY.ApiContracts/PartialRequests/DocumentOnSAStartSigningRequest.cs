namespace NOBY.ApiContracts;
public partial class DocumentOnSAStartSigningRequest : IRequest<DocumentOnSaStartSigningResponse>
{
    [JsonIgnore]
    public int? SalesArrangementId { get; set; }

    public DocumentOnSAStartSigningRequest InfuseSalesArrangementId(int salesArrangementId)
    {
        SalesArrangementId = salesArrangementId;
        return this;
    }

}
