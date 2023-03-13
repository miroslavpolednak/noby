namespace NOBY.Api.Endpoints.Cases.GetCustomers;

public sealed class GetCustomersResponseCustomer
{
    /// <summary>
    /// ID role klienta
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Jméno role - NameNoby z číselníku CustomerRole|RelationshipCustomerProductType
    /// </summary>
    public string? RoleName { get; set; }

    /// <summary>
    /// Zmocněnec pro komunikaci
    /// </summary>
    public bool Agent { get; set; }

    /// <summary>
    /// Příznak o úspěšně doběhlé KYC proceduře
    /// </summary>
    public bool IsKYCSuccessful { get; set; }

    /// <summary>
    /// Jméno
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Příjmení
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Datum narození
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// KBID
    /// </summary>
    public string? KBID { get; set; }

    public SharedDto.IdentificationDocumentFullRequired? IdentificationDocument { get; set; }

    public SharedDto.ContactsConfirmedDto? Contacts { get; set; }

    public DomainServices.CodebookService.Contracts.GenericCodebookItem? CitizenshipCountry { get; set; }

    public CIS.Foms.Types.Address? PermanentAddress { get; set; }

    public CIS.Foms.Types.Address? ContactAddress { get; set; }
}
