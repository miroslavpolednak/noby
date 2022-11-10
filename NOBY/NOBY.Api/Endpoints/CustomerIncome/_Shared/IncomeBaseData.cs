namespace NOBY.Api.Endpoints.CustomerIncome.Dto;

public class IncomeBaseData 
    : BaseIncome
{
    /// <summary>
    /// ID prijmu, pokud se jedna o jiz ulozeny prijem. NULL pokud se jedna o novy prijem.
    /// </summary>
    public int? IncomeId { get; set; }

    public string? IncomeSource { get; set; }

    public bool? HasProofOfIncome { get; set; }

    /// <summary>
    /// Typ prijmu
    /// </summary>
    /// <example>1</example>
    public CIS.Foms.Enums.CustomerIncomeTypes IncomeTypeId { get; set; }
}
