using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.CaseStates;

[DataContract]
public class CaseStateItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    [JsonIgnore]
    public CIS.Foms.Enums.CaseStates Value { get; set; }
    
    [DataMember(Order = 3)]
    public string Name { get; set; }

    [DataMember(Order = 4)]
    [JsonIgnore]
    public bool IsDefault { get; set; }
}
