using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;

[DataContract]
public class RiskApplicationTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public CIS.Foms.Enums.Mandants Mandant { get; set; }

    [DataMember(Order = 3)]
    public List<int> ProductTypeId { get; set; }

    [DataMember(Order = 4)]
    public int? LoanKindId { get; set; }

    [DataMember(Order = 5)]
    public string Name { get; set; }

    [DataMember(Order = 6)]
    public int? LtvFrom { get; set; }

    [DataMember(Order = 7)]
    public int? LtvTo { get; set; }

    [DataMember(Order = 8)]
    public string C4mAplCode { get; set; }

    [DataMember(Order = 9)]
    public string C4mAplTypeId { get; set; }

    [DataMember(Order = 10)]
    public List<int> MarketingActions { get; set; }

    [DataMember(Order = 11)]
    public bool IsValid { get; set; }

    [JsonIgnore]
    public string ProductId;

    [JsonIgnore]
    public string MA;
}
