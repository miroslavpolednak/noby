using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangement.Dto;

public sealed class CustomerChange3602Detail
{
    /// <summary>
    /// ID domacnosti
    /// </summary>
    [Required]
    public int HouseholdId { get; set; }

    /// <summary>
    /// Přistupující manžel/ka?
    /// </summary>
    public bool? IsSpouseInDebt { get; set; }
}
