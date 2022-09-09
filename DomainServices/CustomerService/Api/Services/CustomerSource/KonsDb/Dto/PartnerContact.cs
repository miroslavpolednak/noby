namespace DomainServices.CustomerService.Api.Services.CustomerSource.KonsDb.Dto;

public record PartnerContact
{
    public long ContactId { get; init; }

    public bool IsPrimaryContact { get; init; }

    public byte ContactType { get; init; }

    public string? Value { get; init; }
}