namespace DomainServices.RiskIntegrationService.ExternalServices.Dto;

public class C4mResourceIdentifier
{
    public string Instance { get; set; } = default!;

    public string Domain { get; set; } = default!;

    public string Resource { get; set; } = default!;

    public string Id { get; set; } = default!;

    public string? Variant { get; set; }

    public string ToC4M()
        => string.IsNullOrEmpty(Variant)
        ? $"urn:ri:{Instance}.{Domain}.{Resource}.{Id}"
        : $"urn:ri:{Instance}.{Domain}.{Resource}.{Id}~{Variant}";
}
