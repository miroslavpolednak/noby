namespace NOBY.Api.Endpoints.Customer.GetDetailWithChanges.Dto;

public class TaxResidenceItem
{
    public DateTime? validFrom { get; set; }

    public List<TaxResidenceCountryItem>? ResidenceCountries { get; set; }
}

public class TaxResidenceCountryItem
{
    public int? CountryId { get; set; }

    public string? Tin { get; set; }
}