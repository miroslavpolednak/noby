namespace NOBY.Api.Endpoints.Customer.Shared;

public sealed class TaxResidenceItem
{
    public DateTime? validFrom { get; set; }

    public List<TaxResidenceCountryItem>? ResidenceCountries { get; set; }
}

public sealed class TaxResidenceCountryItem
{
    public int? CountryId { get; set; }

    public string? Tin { get; set; }
}