namespace FOMS.Api.Endpoints.CustomerIncome.GetIncomes;

public class IncomeInList
    : Dto.BaseIncome
{
    /// <summary>
    /// IncomeId
    /// </summary>
    public int IncomeId { get; set; }

    /// <summary>
    /// Typ prijmu
    /// </summary>
    /// <example>1</example>
    public CIS.Foms.Enums.CustomerIncomeTypes IncomeTypeId { get; set; }
}
