namespace NOBY.Api.Endpoints.Codebooks.Dto;

public sealed class DeveloperStatus
{
    /// <summary>
    /// Id statusu developera
    /// </summary>
    public int? StatusId { get; set; }

    /// <summary>
    /// Status developera
    /// </summary>
    public string? StatusText { get; set; }
}
