using System.Text.Json.Serialization;
using NOBY.Api.Endpoints.CustomerIncome.SharedDto;
using NOBY.Dto.Attributes;

namespace NOBY.Api.Endpoints.CustomerIncome.CreateIncome;

public sealed class CreateIncomeRequest
    : SharedDto.BaseIncome, IRequest<int>
{
    [JsonIgnore]
    internal int? CustomerOnSAId;

    public SharedTypes.Enums.CustomerIncomeTypes IncomeTypeId { get; set; }

    /// <summary>
    /// Detailni informace o prijmu
    /// </summary>
    [SwaggerOneOf<IncomeDataEmployement, IncomeDataEntrepreneur, IncomeDataOther>]
    public object? Data { get; set; }

    internal CreateIncomeRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
