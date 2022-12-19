using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.CustomerService.Api.Services.KonsDb.Dto;

public record Partner
{
    [Column("Id")]
    public long PartnerId { get; init; }

    [Column("Jmeno")]
    public string? FirstName { get; init; }

    [Column("Prijmeni")]
    public string? LastName { get; init; }

    [Column("Titul")]
    public string? DegreeBefore { get; init; }

    [Column("Pohlavi")]
    public int GenderId { get; init; }

    [Column("RodneCisloIco")]
    public string? BirthNumber { get; init; }

    [Column("DatumNarozeni")]
    public DateTime? BirthDate { get; init; }

    [Column("MistoNarozeni")]
    public string? PlaceOfBirth { get; init; }

    [Column("PEP")]
    public bool IsPoliticallyExposed { get; init; }

    [Column("StatniPrislusnostId")]
    public int? CitizenshipCountryId { get; init; }

    [Column("PrukazTotoznosti")]
    public string? IdentificationDocumentNumber { get; init; }

    [Column("TypDokladu")]
    public byte IdentificationDocumentTypeId { get; init; }

    [Column("PrukazVydalDatum")]
    public DateTime? IdentificationDocumentIssuedOn { get; init; }

    [Column("PrukazVydal")]
    public string? IdentificationDocumentIssuedBy { get; init; }

    [Column("PrukazStatVydaniId")]
    public int? IdentificationDocumentIssuingCountryId { get; init; }

    [Column("PreukazPlatnostDo")]
    public DateTime? IdentificationDocumentValidTo { get; init; }

    [Column("Ulice")]
    public string? Street { get; init; }

    [Column("CisloDomu1")]
    public string? HouseNumber { get; init; }

    [Column("CisloDomu2")]
    public string? StreetNumber { get; init; }

    [Column("Psc")]
    public string? PostCode { get; init; }

    [Column("Misto")]
    public string? City { get; init; }

    [Column("VypisyUlice")]
    public string? MailingStreet { get; init; }

    [Column("VypisyCisloDomu2")]
    public string? MailingHouseNumber { get; init; }

    [Column("VypisyCisloDomu1")]
    public string? MailingStreetNumber { get; init; }

    [Column("VypisyPsc")]
    public string? MailingPostCode { get; init; }

    [Column("VypisyMisto")]
    public string? MailingCity { get; init; }

    [Column("KBId")]
    public long? KbId { get; init; }

    public ICollection<PartnerContact> Contacts { get; set; } = new List<PartnerContact>();
}