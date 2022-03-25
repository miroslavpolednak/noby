using System.Text.Json.Serialization;

namespace FOMS.Api.Endpoints.CustomerIncome.UpdateIncome;

public class UpdateIncomeRequest
    : Dto.BaseIncome, IRequest
{
    [JsonIgnore]
    public int IncomeId { get; set; }

    [JsonIgnore]
    public int CustomerOnSAId { get; set; }

    internal UpdateIncomeRequest InfuseId(int customerOnSAId, int incomeId)
    {
        this.CustomerOnSAId = customerOnSAId;
        this.IncomeId = incomeId;
        return this;
    }
}
