using System.Text.Json.Serialization;

namespace FOMS.Api.Endpoints.CustomerIncome.CreateIncomes;

public class CreateIncomesRequest
    : IRequest<int[]>
{
    [JsonIgnore]
    public int CustomerOnSAId { get; set; }

    /// <summary>
    /// Seznam prijmu (novych, existujicich) pro daneho customera
    /// </summary>
    public List<CreateIncomeItem>? Incomes { get; set; }

    internal CreateIncomesRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
