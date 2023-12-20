namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.Dto;

public sealed class Household
{
    /// <summary>
    /// ID domácnosti
    /// </summary>
    public long? HouseholdId { get; set; }

    /// <summary>
    /// Risková data
    /// </summary>
    public HouseholdRisk Risk { get; set; } = null!;

    /// <summary>
    /// Závazky
    /// </summary>
    public List<HouseholdCustomerObligations>? CustomerObligations { get; set; }
}