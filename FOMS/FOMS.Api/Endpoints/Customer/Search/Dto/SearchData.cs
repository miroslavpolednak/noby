namespace FOMS.Api.Endpoints.Customer.Search.Dto;

public class SearchData
    : BaseCustomer
{
    /// <summary>
    /// [optional] ID klienta v MP nebo KB
    /// </summary>
    public long? IdentityId { get; set; }

    /// <summary>
    /// [optional] Zeme vydani dokladu
    /// </summary>
    public int? IssuingCountryId { get; set; }

    /// <summary>
    /// [optional] Type dokladu
    /// </summary>
    public int? IdentificationDocumentTypeId { get; set; }

    /// <summary>
    /// [optional] Cislo dokladu
    /// </summary>
    public string? IdentificationDocumentNumber { get; set; }
}