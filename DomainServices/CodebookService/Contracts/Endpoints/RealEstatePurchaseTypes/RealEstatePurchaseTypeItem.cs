using System.Text.Json.Serialization;
namespace DomainServices.CodebookService.Contracts.Endpoints.RealEstatePurchaseTypes;

[DataContract]
public class RealEstatePurchaseTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    [JsonIgnore]
    public bool IsValid { get; set; }

    [DataMember(Order = 4)]
    public bool IsDefault { get; set; }

    [DataMember(Order = 5)]
    [JsonIgnore]
    public int Order { get; set; }
}