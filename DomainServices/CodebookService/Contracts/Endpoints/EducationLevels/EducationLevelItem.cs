// using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.EducationLevels;

[DataContract]
public class EducationLevelItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }


    [DataMember(Order = 2)]
    public string Name { get; set; }


    [DataMember(Order = 3)]
    // TODO: ačkoliv se to nemá propagovat na FE API, používá to aktuálně RIP na CodebooksAPI !?
    // [JsonIgnore]
    public string RdmCode { get; set; }


    [DataMember(Order = 4)]
    // TODO: ačkoliv se to nemá propagovat na FE API, používá to aktuálně RIP na CodebooksAPI !?
    // [JsonIgnore]
    public int ScoringCode { get; set; }
}
