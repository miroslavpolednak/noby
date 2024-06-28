namespace NOBY.ApiContracts;

public partial class CustomerObligationCreateObligationRequest
    : IRequest<int>
{
    [JsonIgnore]
    public int? CustomerOnSAId { get; private set; }

    public CustomerObligationCreateObligationRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
