namespace FOMS.Api.Endpoints.Offer.Dto;

/// <summary>
/// Instance modelace
/// </summary>
internal record OfferInstance(
    int OfferInstanceId,
    BuildingSavingsInput Input,
    BuildingSavingsData SavingsData,
    LoanData? LoanData = null
)
{ }
