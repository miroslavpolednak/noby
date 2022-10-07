namespace FOMS.Api.Endpoints.Product.GetCustomersOnProduct;

public sealed class GetCustomersOnProductCustomer
{
    public List<CIS.Foms.Types.CustomerIdentity>? Identities { get; set; }

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
    /// Číslo dokladu
    /// </summary>
    public string? DocumentNumber { get; set; }

    public DomainServices.CodebookService.Contracts.GenericCodebookItem? DocumentType { get; set; }
}
