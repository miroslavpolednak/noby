namespace NOBY.Api.Endpoints.Address.KatuzSearch.Dto;

public class KatuzLine
{
    /// <summary>
    /// Id katastrálního území (KATUZ)
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Textová reprezentace katastrálního území
    /// </summary>
    public string Title { get; set; } = null!;
}