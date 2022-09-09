namespace DomainServices.CustomerService.Api.Services.CustomerSource.KonsDb.Dto;

public record Partner
{
    public long PartnerId { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public int GenderId { get; init; }

    public string? BirthNumber { get; init; }

    public DateTime? BirthDate { get; init; }

    public string? DegreeBefore { get; init; }

    public string? PlaceOfBirth { get; init; }

    public bool IsPoliticallyExposed { get; init; }

    public int? CitizenshipCountryId { get; init; }

    public string? IdentificationDocumentNumber { get; init; }

    public byte IdentificationDocumentTypeId { get; init; }

    public DateTime? IdentificationDocumentIssuedOn { get; init; }

    public string? IdentificationDocumentIssuedBy { get; init; }

    public int? IdentificationDocumentIssuingCountryId { get; init; }

    public DateTime? IdentificationDocumentValidTo { get; init; }

    public string? Street { get; init; }

    public string? HouseNumber { get; init; }

    public string? StreetNumber { get; init; }

    public string? PostCode { get; init; }

    public string? City { get; init; }

    public string? MailingStreet { get; init; }

    public string? MailingHouseNumber { get; init; }

    public string? MailingStreetNumber { get; init; }

    public string? MailingPostCode { get; init; }

    public string? MailingCity { get; init; }

    public List<PartnerContact> Contacts { get; } = new();
}