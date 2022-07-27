using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.CustomerProfiles;

[DataContract]
public class CustomerProfileItem
{   
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    [JsonIgnore]
    public CIS.Foms.Enums.CustomerProfiles EnumValue { get; set; }

    [DataMember(Order = 3)]
    [JsonIgnore]
    public string Code { get; set; }

    [DataMember(Order = 4)]
    public string Name { get; set; }
}
