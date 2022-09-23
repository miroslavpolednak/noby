namespace FOMS.Api.Endpoints.Customer.Create.Dto;

public class IdentificationDocument
{
    /// <summary>
    /// Stát vydání dokladu
    /// </summary>
    public int IssuingCountryId { get; set; }

    /// <summary>
    /// Typ dokladu
    /// </summary>
    public int IdentificationDocumentTypeId { get; set; }

    /// <summary>
    /// Číslo dokladu
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Doklad vydal
    /// </summary>
    public string IssuedBy { get; set; }
}
