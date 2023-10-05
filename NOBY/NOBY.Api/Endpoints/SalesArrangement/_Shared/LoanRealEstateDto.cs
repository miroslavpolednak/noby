namespace NOBY.Api.Endpoints.SalesArrangement.Dto;

public class LoanRealEstateDto
{
    /// <summary>
    /// Typ nemovitosti
    /// </summary>
    public int RealEstateTypeId { get; set; }

    /// <summary>
    /// Slouží k zajištění
    /// </summary>
    public bool IsCollateral { get; set; }

    /// <summary>
    /// Účel pořízení nemovitosti
    /// </summary>
    public int RealEstatePurchaseTypeId { get; set; }
}
