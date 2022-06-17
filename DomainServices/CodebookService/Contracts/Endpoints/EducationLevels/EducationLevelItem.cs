namespace DomainServices.CodebookService.Contracts.Endpoints.EducationLevels;

[DataContract]
public class EducationLevelItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public string RDMCode { get; set; }
}
