namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.V2.Dto;

public sealed class Household
{
    /// <summary>
    /// ID domácnosti
    /// </summary>
    public long HouseholdId { get; set; }

    /// <summary>
    /// Typ domácnosti
    /// </summary>
    public int HouseholdTypeId { get; set; }

    /// <summary>
    /// Risková data
    /// </summary>
    public HouseholdRisk? Risk { get; set; } = null!;

    /// <summary>
    /// Závazky
    /// </summary>
    public List<HouseholdCustomerObligations>? Customers { get; set; }
}