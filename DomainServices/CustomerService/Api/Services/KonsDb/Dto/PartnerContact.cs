using System.ComponentModel.DataAnnotations.Schema;

namespace DomainServices.CustomerService.Api.Services.KonsDb.Dto;

public record PartnerContact
{
    [Column("Id")]
    public long ContactId { get; init; }

    [Column("PrimarniKontakt")]
    public bool IsPrimaryContact { get; init; }

    [Column("TypKontaktu")]
    public byte ContactType { get; init; }

    public string? Value { get; init; }
}