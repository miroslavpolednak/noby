using System.Text.Json.Serialization;

namespace NOBY.Api.Endpoints.CustomerIncome.CreateIncome;

public sealed class CreateIncomeRequest
    : Dto.BaseIncome, IRequest<int>
{
    [JsonIgnore]
    internal int? CustomerOnSAId;

    public CIS.Foms.Enums.CustomerIncomeTypes IncomeTypeId { get; set; }

    /// <summary>
    /// Detailni informace o prijmu
    /// </summary>
    public object? Data { get; set; }

    internal CreateIncomeRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
