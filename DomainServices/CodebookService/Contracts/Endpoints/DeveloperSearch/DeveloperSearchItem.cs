namespace DomainServices.CodebookService.Contracts.Endpoints.DeveloperSearch;

[DataContract]
public class DeveloperSearchItem
{
    /// <summary>
    /// ID developera
    /// </summary>
    [DataMember(Order = 1)]
    public int DeveloperId { get; set; }

    /// <summary>
    /// ID developerského projektu
    /// </summary>
    [DataMember(Order = 2)]
    public int DeveloperProjectId { get; set; }

    /// <summary>
    /// Popis developera
    /// </summary>
    [DataMember(Order = 3)]
    public string Description { get; set; }
}
