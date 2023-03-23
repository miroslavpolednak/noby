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

    /// <summary>
    /// Id statusu developera
    /// </summary>
    public int? StatusId { get; set; }

    /// <summary>
    /// Status developera
    /// </summary>
    public string? StatusText { get; set; }

    /// <summary>
    /// Balíček benefitu
    /// </summary>
    public bool ShowBenefitsPackage { get; set; }

    /// <summary>
    /// Benefity nad rámec balíčku
    /// </summary>
    public bool ShowBenefitsBeyondPackage { get; set; }
}