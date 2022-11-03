namespace NOBY.Api.Endpoints.Offer.CreateMortgageCase;

public sealed class CreateMortgageCaseResponse
{
    /// <summary>
    /// ID noveho Case
    /// </summary>
    public long CaseId { get; set; }
    
    /// <summary>
    /// ID noveho Sales Arrangement
    /// </summary>
    public int SalesArrangementId { get; set; }
    
    /// <summary>
    /// ID simulace pro kterou Case vznikl
    /// </summary>
    public int OfferId { get; set; }

    /// <summary>
    /// ID default domacnosti
    /// </summary>
    public int HouseholdId { get; set; }

    /// <summary>
    /// ID noveho klienta
    /// </summary>
    public int CustomerOnSAId { get; set; }
}
