namespace FOMS.Api.Endpoints.Customer.Search.Dto;

public class SearchData
    : BaseCustomer
{
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