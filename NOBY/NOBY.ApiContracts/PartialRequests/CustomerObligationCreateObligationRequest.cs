namespace NOBY.ApiContracts;

public partial class CustomerObligationCreateObligationRequest
    : IRequest<int>
{
    [JsonIgnore]
    public int? CustomerOnSAId;

    public CustomerObligationCreateObligationRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
