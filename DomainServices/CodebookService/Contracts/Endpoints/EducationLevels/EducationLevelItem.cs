using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.EducationLevels;

[DataContract]
public class EducationLevelItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }


    [DataMember(Order = 2)]
    public string Name { get; set; }


    [DataMember(Order = 3)]
    [JsonIgnore]
    public string RdmCode { get; set; }


    [DataMember(Order = 4)]
    [JsonIgnore]
    public int ScoringCode { get; set; }

    
    [DataMember(Order = 5)]
    public string ShortName { get; set; }


    [DataMember(Order = 6)]
    public bool IsValid { get; set; }

}
