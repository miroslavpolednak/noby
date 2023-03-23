namespace NOBY.Api.Endpoints.Codebooks.Dto;

public sealed class Developer
{
    /// <summary>
    /// Jméno developera
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// ICO/RČ developera
    /// </summary>
    public string? Cin { get; set; }

    public DeveloperStatus? Status { get; set; }

    /// <summary>
    /// Balíček benefitu
    /// </summary>
    public bool ShowBenefitsPackage { get; set; }

    /// <summary>
    /// Benefity nad rámec balíčku
    /// </summary>
    public bool ShowBenefitsBeyondPackage { get; set; }
}