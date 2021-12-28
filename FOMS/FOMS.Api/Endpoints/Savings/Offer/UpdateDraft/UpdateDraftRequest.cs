namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal class UpdateDraftRequest
    : IRequest<SaveCaseResponse>
{
    public int OfferInstanceId { get; set; }

    public int SalesArrangementId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }
}
