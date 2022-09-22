namespace FOMS.Api.Endpoints.Customer.Create.Dto;

public class IdentificationDocument
{
    /// <summary>
    /// Stát vydání dokladu
    /// </summary>
    public int issuingCountryCode { get; set; }

    /// <summary>
    /// Typ dokladu
    /// </summary>
    public int typeCode { get; set; }

    /// <summary>
    /// Číslo dokladu
    /// </summary>
    public string documentNumber { get; set; }

    /// <summary>
    /// Doklad vydal
    /// </summary>
    public string issuedBy { get; set; }
}
