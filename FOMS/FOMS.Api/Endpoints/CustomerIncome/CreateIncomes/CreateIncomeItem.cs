namespace FOMS.Api.Endpoints.CustomerIncome.CreateIncomes;

public class CreateIncomeItem 
    : Dto.BaseIncome
{
    /// <summary>
    /// ID prijmu, pokud se jedna o jiz ulozeny prijem. NULL pokud se jedna o novy prijem.
    /// </summary>
    public int? IncomeId { get; set; }

    /// <summary>
    /// Typ prijmu
    /// </summary>
    /// <example>1</example>
    public CIS.Foms.Enums.CustomerIncomeTypes IncomeTypeId { get; set; }
}
