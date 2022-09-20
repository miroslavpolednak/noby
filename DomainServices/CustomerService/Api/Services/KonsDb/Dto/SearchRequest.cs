namespace DomainServices.CustomerService.Api.Services.KonsDb.Dto;

public record SearchRequest
{
    public bool IsEmptyRequest => !SearchByIds && new SearchRequest { PartnerIds = PartnerIds }.Equals(this);

    public List<long> PartnerIds { get; init; } = new();

    public bool SearchByIds => PartnerIds.Any();

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public string? BirthNumber { get; init; }

    public DateTime? DateOfBirth { get; init; }

    public string? DocumentNumber { get; init; }

    public int? DocumentTypeId { get; init; }

    public int? DocumentIssuingCountryId { get; init; }
}