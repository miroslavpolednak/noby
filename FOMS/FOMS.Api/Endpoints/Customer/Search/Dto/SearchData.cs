namespace FOMS.Api.Endpoints.Customer.Search.Dto;

public class SearchData
    : BaseCustomer
{
    /// <summary>
    /// ID klienta v MP nebo KB
    /// </summary>
    public int? IdentityId { get; set; }

    /// <summary>
    /// Zeme vydani dokladu
    /// </summary>
    public int? IssuingCountryId { get; set; }
    
    /// <summary>
    /// Type dokladu
    /// </summary>
    public string? IdentificationDocumentTypeId { get; set; }
    
    /// <summary>
    /// Cislo dokladu
    /// </summary>
    public string? IdentificationDocumentNumber { get; set; }
}