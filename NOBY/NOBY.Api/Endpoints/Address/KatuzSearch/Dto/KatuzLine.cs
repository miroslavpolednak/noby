namespace NOBY.Api.Endpoints.Address.KatuzSearch.Dto;

public class KatuzLine
{
    /// <summary>
    /// Id katastrálního území (KATUZ)
    /// </summary>
    public long KatuzId { get; set; }

    /// <summary>
    /// Textová reprezentace katastrálního území
    /// </summary>
    public string KatuzTitle { get; set; } = null!;
}