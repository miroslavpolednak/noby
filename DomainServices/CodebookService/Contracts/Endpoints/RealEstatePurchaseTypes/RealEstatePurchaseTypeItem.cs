using System.Runtime.Intrinsics.X86;
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
    public bool IsValid { get; set; }

    [DataMember(Order = 4)]
    public bool IsDefault { get; set; }

    [DataMember(Order = 5)]
    [JsonIgnore]
    public int Order { get; set; }

    [DataMember(Order = 6)]
    public string Code { get; set; }

    [DataMember(Order = 7)]
    public int? MandantId { get; set; }
}
