namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal sealed class GetDataResponse
{
    public int OfferInstanceId { get; set; }

    public BuildingSavingsData? BuildingSavings { get; set; }
    
    public LoanData? Loan { get; set; }

    public DateTime CreatedTime { get; set; }

    public int CreatedUserId { get; set; }

    public string? CreatedUserName { get; set; }

    public BuildingSavingsInput? InputData { get; set; }
}
