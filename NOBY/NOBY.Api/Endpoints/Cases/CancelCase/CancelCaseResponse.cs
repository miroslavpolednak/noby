using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Cases.CancelCase;

public class CancelCaseResponse
{
    /// <summary>
    /// Stavy Case
    /// </summary>
    public State State { get; set; }
    
    /// <summary>
    /// Slovne nazev stavu Case
    /// </summary>
    public string? StateName { get; set; }
    
    /// <summary>
    /// Účely úvěru
    /// </summary>
    public List<CustomerOnSAItem>? CustomersOnSa { get; set; }
}

public enum State
{
    Unknown = 0,
    InProgress = 1,
    InApproval = 2,
    InSigning = 3,
    InDisbursement = 4,
    InAdministration = 5,
    Finished = 6,
    Cancelled = 7,
    InApprovalConfirmed = 8,
    ToBeCancelled = 9,
    ToBeCancelledConfirmed = 10
}

public class CustomerOnSAItem
{
    /// <summary>
    /// Id customera na sales arrangementu
    /// </summary>
    /// <example>123456</example>
    public int CustomerOnSAId { get; set; }

    /// <summary>
    /// Jméno
    /// </summary>
    /// <example>Jidáš</example>
    [MinLength(1)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Příjmení
    /// </summary>
    /// <example>Skočdopole</example>
    [MinLength(1)]
    public string LastName { get; set; } = null!;
    
    /// <summary>
    /// Datum narození
    /// </summary>
    /// <example>2000-02-01</example>
    public DateTime BirthDate { get; set; }
}