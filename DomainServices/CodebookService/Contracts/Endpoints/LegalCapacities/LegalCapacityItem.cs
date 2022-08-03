using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.LegalCapacities;

[DataContract]
public class LegalCapacityItem
{   
    [DataMember(Order = 1)]
    public int Id { get; set; }


    [DataMember(Order = 2)]
    public CIS.Foms.Enums.LegalCapacities Code { get; set; }

    [DataMember(Order = 3)]
    public string Name { get; set; }


    [DataMember(Order = 4)]
    [JsonIgnore]
    public string Description { get; set; }
}
