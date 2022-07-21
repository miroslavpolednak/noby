namespace DomainServices.CodebookService.Contracts.Endpoints.DeveloperSearch;

[DataContract]
public class DeveloperSearchItem
{
    [DataMember(Order = 1)]
    public int DeveloperId { get; set; }

    [DataMember(Order = 2)]
    public int DeveloperProjectId { get; set; }

    [DataMember(Order = 3)]
    public string DeveloperName { get; set; }

    [DataMember(Order = 4)]
    public string DeveloperProjectName { get; set; }

    [DataMember(Order = 5)]
    public string DeveloperCIN { get; set; }
}
