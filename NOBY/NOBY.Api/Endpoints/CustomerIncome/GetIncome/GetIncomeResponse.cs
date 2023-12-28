using NOBY.Api.Endpoints.CustomerIncome.SharedDto;
using NOBY.Dto.Attributes;

namespace NOBY.Api.Endpoints.CustomerIncome.GetIncome;

public sealed class GetIncomeResponse
    : SharedDto.BaseIncome
{
    /// <summary>
    /// Druh prijmu
    /// </summary>
    public int IncomeTypeId { get; set; }

    /// <summary>
    /// Detail prijmu na L2
    /// </summary>
    [SwaggerOneOf<IncomeDataEmployement, IncomeDataEntrepreneur, IncomeDataOther>]
    public object? Data { get; set; }
}
