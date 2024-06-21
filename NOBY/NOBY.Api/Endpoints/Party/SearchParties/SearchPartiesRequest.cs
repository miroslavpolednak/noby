namespace NOBY.Api.Endpoints.Party.SearchParties;

public class SearchPartiesRequest : IRequest<List<SearchPartiesResponse>>
{
    /// <summary>
    /// Id země z číselníku států
    /// </summary>
    public int? CountryId { get; set; }

    /// <summary>
    /// IČ společnosti
    /// </summary>
    public string? Cin { get; set; }

    /// <summary>
    /// Pattern, podle kterého vyhledáváme
    /// </summary>
    public string? SearchText { get; set; }
}
