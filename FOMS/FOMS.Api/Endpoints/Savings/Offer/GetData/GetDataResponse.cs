namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal sealed class GetDataResponse
{
    public int OfferInstanceId { get; set; }

    public BuildingSavingsData? BuildingSavings { get; set; }
    
    public LoanData? Loan { get; set; }

    public DateTime InsertTime { get; set; }

    public int? InsertUserId { get; set; }

    public DomainServices.OfferService.Contracts.SimulationTypes SimulationType { get; set; }

    public BuildingSavingsInput? InputData { get; set; }
}
