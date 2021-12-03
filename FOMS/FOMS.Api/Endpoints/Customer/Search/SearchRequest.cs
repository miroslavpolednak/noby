namespace FOMS.Api.Endpoints.Customer.Dto;

internal sealed class SearchRequest
    : IRequest<List<SearchResponse>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BirthNumber { get; set; }
    public DateTime? dateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int? IdentificationDocumentType { get; set; }
    public string? IdentificationDocumentNumber { get; set; }
    public string? TaxId { get; set; }
    public bool IsNaturalPerson { get; set; }
}
