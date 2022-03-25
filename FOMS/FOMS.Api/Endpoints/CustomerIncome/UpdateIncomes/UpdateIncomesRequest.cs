using System.Text.Json.Serialization;

namespace FOMS.Api.Endpoints.CustomerIncome.UpdateIncomes;

public class UpdateIncomesRequest
    : IRequest<int[]>
{
    [JsonIgnore]
    public int CustomerOnSAId { get; set; }

    /// <summary>
    /// Seznam prijmu (novych, existujicich) pro daneho customera
    /// </summary>
    public List<Dto.IncomeBaseData>? Incomes { get; set; }

    internal UpdateIncomesRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
