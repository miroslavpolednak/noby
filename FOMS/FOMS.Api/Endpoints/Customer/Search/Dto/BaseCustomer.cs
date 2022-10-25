namespace FOMS.Api.Endpoints.Customer.Search.Dto;

public abstract class BaseCustomer
{
    /// <summary>
    /// Jmeno klienta
    /// </summary>
    public string? FirstName { get; set; }
    
    /// <summary>
    /// Prijmeni klienta
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// Datum narozeni FO
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
    
    /// <summary>
    /// Rodne cislo
    /// </summary>
    public string? BirthNumber { get; set; }
    
    /// <summary>
    /// Nazev pravnicke osoby
    /// </summary>
    public string? NameJuridicalPerson { get; set; }
    
    /// <summary>
    /// IC pravnicke osoby
    /// </summary>
    public string? Cin { get; set; }
}