namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.V2.Dto;

public sealed class HouseholdCustomerObligations
{
    /// <summary>
    /// Jméno klienta
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Příjmení klienta
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Datum narození klienta
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Role klienta
    /// </summary>
    public CustomerRoles RoleId { get; set; }

    /// <summary>
    /// Existující závazky
    /// </summary>
    public List<HouseholdObligationItem>? ExistingObligations { get; set; }

    /// <summary>
    /// Rozpracované závazky
    /// </summary>
    public List<HouseholdObligationItem>? RequestedObligations { get; set; }
}