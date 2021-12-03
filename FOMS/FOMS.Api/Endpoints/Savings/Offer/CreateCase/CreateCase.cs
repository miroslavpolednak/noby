namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal class CreateCase
{
    public int? PartyId { get; set; }

    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
}
