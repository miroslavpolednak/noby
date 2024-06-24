namespace NOBY.ApiContracts;

public partial class CustomerIncomeUpdateIncomeRequest
    : IRequest
{
    [JsonIgnore]
    public int IncomeId { get; set; }

    [JsonIgnore]
    public int CustomerOnSAId { get; set; }

    public CustomerIncomeUpdateIncomeRequest InfuseId(int customerOnSAId, int incomeId)
    {
        this.CustomerOnSAId = customerOnSAId;
        this.IncomeId = incomeId;
        return this;
    }
}
