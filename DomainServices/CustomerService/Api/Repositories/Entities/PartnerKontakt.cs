namespace DomainServices.CustomerService.Api.Repositories.Entities;

internal record PartnerKontakt
{
    public int KontaktId { get; init; }

    public bool PrimarniKontakt { get; init; }

    public int TypKontaktu {  get; init; }

    public string? Value {  get; init; }
}