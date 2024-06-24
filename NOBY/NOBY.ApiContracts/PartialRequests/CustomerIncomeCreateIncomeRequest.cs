namespace NOBY.ApiContracts;

public partial class CustomerIncomeCreateIncomeRequest
    : IRequest<int>
{
    [JsonIgnore]
    public int? CustomerOnSAId;

    public CustomerIncomeCreateIncomeRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
