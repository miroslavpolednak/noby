namespace DomainServices.CodebookService.Contracts.Endpoints.DeveloperSearch;

[DataContract]
public class DeveloperSearchItem
{
    /// <summary>
    /// ID developera
    /// </summary>
    [DataMember(Order = 1)]
    public int? DeveloperId { get; set; }

    /// <summary>
    /// ID developerského projektu
    /// </summary>
    [DataMember(Order = 2)]
    public int? DeveloperProjectId { get; set; }

    /// <summary>
    /// Jméno developera
    /// </summary>
    [DataMember(Order = 3)]
    public string? DeveloperName { get; set; }

    /// <summary>
    /// Jméno developerského projektu
    /// </summary>
    [DataMember(Order = 4)]
    public string? DeveloperProjectName { get; set; }

    /// <summary>
    /// ICO/RČ developera
    /// </summary>
    [DataMember(Order = 5)]
    public string? DeveloperCIN { get; set; }
}
