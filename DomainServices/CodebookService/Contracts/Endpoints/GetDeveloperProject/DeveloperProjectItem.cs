namespace DomainServices.CodebookService.Contracts.Endpoints.GetDeveloperProject;

[DataContract]
public class DeveloperProjectItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public int DeveloperId { get; set; }

    [DataMember(Order = 3)]
    public string Name { get; set; }

    [DataMember(Order = 4)]
    public string WarningForKb { get; set; }

    [DataMember(Order = 5)]
    public string WarningForMp { get; set; }

    [DataMember(Order = 6)]
    public string Web { get; set; }

    [DataMember(Order = 7)]
    public int MassValuation { get; set; }

    [DataMember(Order = 8)]
    public string Recommandation { get; set; }

    [DataMember(Order = 9)]
    public string Place { get; set; }

    [DataMember(Order = 10)]
    public bool IsValid { get; set; }
}
