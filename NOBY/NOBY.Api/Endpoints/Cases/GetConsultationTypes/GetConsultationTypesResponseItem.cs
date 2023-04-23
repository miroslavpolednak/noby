namespace NOBY.Api.Endpoints.Cases.GetConsultationTypes;

public sealed class GetConsultationTypesResponseItem
{
    /// <summary>
    /// ID typu konzultace
    /// </summary>
    /// <example>0</example>
    public int TaskSubtypeId { get; set; }

    /// <summary>
    /// Název typu konzultace
    /// </summary>
    /// <example>Dotaz (obecný)</example>
    public string taskSubtypeName { get; set; } = string.Empty;
}
