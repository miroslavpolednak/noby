namespace FOMS.Api.Endpoints.CustomerIncome.GetIncome;

public sealed class GetIncomeResponse
    : Dto.BaseIncome
{
    /// <summary>
    /// Druh prijmu
    /// </summary>
    public int IncomeTypeId { get; set; }

    /// <summary>
    /// Detail prijmu na L2
    /// </summary>
    public object? Data { get; set; }
}
