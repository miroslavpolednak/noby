using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementStates;

[DataContract]
public class SalesArrangementStateItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }
    

    [DataMember(Order = 2)]
    [JsonIgnore]
    public CIS.Foms.Enums.SalesArrangementStates EnumValue { get; set; }
    

    [DataMember(Order = 3)]
    public string Name { get; set; }


    [DataMember(Order = 4)]
    [JsonIgnore]
    public int? StarbuildId { get; set; }


    [DataMember(Order = 5)]
    [JsonIgnore]
    public bool IsDefault { get; set; }
}
