using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.SignatureTypes;

[DataContract]
public class SignatureTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    [JsonIgnore]
    public CIS.Foms.Enums.SignatureTypes Value { get; set; }
    
    [DataMember(Order = 3)]
    public string Name { get; set; }
}