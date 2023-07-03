using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NOBY.Api.Endpoints.CustomerIncome.Dto;
using NOBY.Dto.Attributes;

namespace NOBY.Api.Endpoints.CustomerIncome.UpdateIncome;

public sealed class UpdateIncomeRequest
    : Dto.BaseIncome, IRequest
{
    [JsonIgnore]
    public int IncomeId { get; set; }

    [JsonIgnore]
    public int CustomerOnSAId { get; set; }

    [Required]
    public CIS.Foms.Enums.CustomerIncomeTypes IncomeTypeId { get; set; }

    /// <summary>
    /// Detailni informace o prijmu
    /// </summary>
    [SwaggerOneOf<IncomeDataEmployement, IncomeDataEntrepreneur, IncomeDataOther>]
    public object? Data { get; set; }

    internal UpdateIncomeRequest InfuseId(int customerOnSAId, int incomeId)
    {
        this.CustomerOnSAId = customerOnSAId;
        this.IncomeId = incomeId;
        return this;
    }
}
