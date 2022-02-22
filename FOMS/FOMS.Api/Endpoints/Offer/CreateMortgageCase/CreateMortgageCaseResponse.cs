namespace FOMS.Api.Endpoints.Offer.CreateMortgageCase;

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
}
