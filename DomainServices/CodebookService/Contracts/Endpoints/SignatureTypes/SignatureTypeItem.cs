using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.SignatureTypes;

[DataContract]
public class SignatureTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    [JsonIgnore]
    public CIS.Foms.Enums.SignatureTypes EnumValue { get; set; }
    
    [DataMember(Order = 3)]
    public string Name { get; set; }

    [JsonIgnore]
    [DataMember(Order = 4)]
    public bool IsDefault { get; set; }
}