using System.ComponentModel.DataAnnotations;
using CIS.Foms.Enums;

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
}