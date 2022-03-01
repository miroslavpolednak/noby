namespace FOMS.Api.Endpoints.Customer.Search.Dto;

public abstract class BaseCustomer
{
    /// <summary>
    /// Hledani podkle konkretniho ID klienta v MP nebo KB
    /// </summary>
    public CIS.Foms.Types.CustomerIdentity? Identity { get; set; }
    
    /// <summary>
    /// Hledat ve jmene klienta
    /// </summary>
    public string? FirstName { get; set; }
    
    /// <summary>
    /// Hledat v prijmeni klienta
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// Hledat podle data narozeni
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
    
    /// <summary>
    /// Hledat podle rodneho cisla
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
    
    /// <summary>
    /// Email
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Telefon
    /// </summary>
    public string? Phone { get; set; }
}