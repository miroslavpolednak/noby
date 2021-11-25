namespace FOMS.Api.Endpoints.Offer.Dto;

internal sealed class GetBuildingSavingsDataResponse
{
    public int OfferInstanceId { get; set; }

    public BuildingSavingsData? BuildingSavings { get; set; }
    
    public LoanData? Loan { get; set; }

    public DateTime InsertTime { get; set; }

    public int? InsertUserId { get; set; }

    public DomainServices.OfferService.Contracts.SimulationTypes SimulationType { get; set; }

    public BuildingSavingsInput? InputData { get; set; }
}
