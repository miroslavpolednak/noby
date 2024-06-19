using System.ComponentModel.DataAnnotations;

namespace NOBY.Api.Endpoints.Cases.CancelCase;

public class CancelCaseResponse
{
    /// <summary>
    /// Stavy Case
    /// </summary>
    public CaseStates State { get; set; }
    
    /// <summary>
    /// Slovne nazev stavu Case
    /// </summary>
    public string? StateName { get; set; }
    
    /// <summary>
    /// Účely úvěru
    /// </summary>
    public List<CustomerOnSAItem>? CustomersOnSa { get; set; }
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

    /// <summary>
    /// eArchiv ID dokumentu s potvrzení pro daného klienta
    /// </summary>
    /// <example>KBH00000000000000000123456789</example>
    public string DocumentId { get; set; } = string.Empty!;
}