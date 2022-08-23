using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.DrawingTypes;

[DataContract]
public class DrawingTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }
    

    [DataMember(Order = 2)]
    [JsonIgnore]
    public CIS.Foms.Enums.DrawingTypes EnumValue { get; set; }
    

    [DataMember(Order = 3)]
    public string Name { get; set; }


    [DataMember(Order = 4)]
    [JsonIgnore]
    public int? StarbuildId { get; set; }
}
