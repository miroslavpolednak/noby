namespace NOBY.ApiContracts;

public partial class CustomerObligationUpdateObligationRequest
    : IRequest
{
    [JsonIgnore]
    public int ObligationId { get; set; }

    [JsonIgnore]
    public int CustomerOnSAId { get; set; }

    public CustomerObligationUpdateObligationRequest InfuseId(int customerOnSAId, int incomeId)
    {
        this.CustomerOnSAId = customerOnSAId;
        this.ObligationId = incomeId;
        return this;
    }
}
