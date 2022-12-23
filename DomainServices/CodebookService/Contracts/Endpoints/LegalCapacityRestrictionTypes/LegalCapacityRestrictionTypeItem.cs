using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.LegalCapacityRestrictionTypes;

[DataContract]
public class LegalCapacityRestrictionTypeItem
{   
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    [JsonIgnore]
    public CIS.Foms.Enums.LegalCapacityRestrictions EnumValue { get; set; }

    [DataMember(Order = 3)]
    public string RdmCode { get; set; }

    [DataMember(Order = 4)]
    public string Name { get; set; }


    [DataMember(Order = 5)]
    [JsonIgnore]
    public string Description { get; set; }
}
