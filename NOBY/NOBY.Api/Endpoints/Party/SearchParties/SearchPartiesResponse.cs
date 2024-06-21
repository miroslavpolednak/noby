namespace NOBY.Api.Endpoints.Party.SearchParties;

public class SearchPartiesResponse
{
    /// <summary>
    /// Název společnosti
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// IČO
    /// </summary>
    public string Cin { get; set; } = null!;
}
