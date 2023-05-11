using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;

[DataContract]
public class ProductTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public int? MandantId { get; set; }

    [DataMember(Order = 4)]
    public bool IsValid { get; set; }

    [JsonIgnore]
    [DataMember(Order = 5)]
    public int Order { get; set; }

    [DataMember(Order = 6)]
    public int? LoanAmountMin { get; set; }

    [DataMember(Order = 7)]
    public int? LoanAmountMax { get; set; }

    [DataMember(Order = 8)]
    public int? LoanDurationMin { get; set; }

    [DataMember(Order = 9)]
    public int? LoanDurationMax { get; set; }

    [DataMember(Order = 10)]
    public int? LtvMin { get; set; }

    [DataMember(Order = 11)]
    public int? LtvMax { get; set; }

    [JsonIgnore]
    [DataMember(Order = 12)]
    public string MpHomeApiLoanType { get; set; }

    [DataMember(Order = 13)]
    public List<int> LoanKindIds { get; set; }
    
    [JsonIgnore]
    [DataMember(Order = 14)]
    public int? KonsDbLoanType { get; set; }

    [DataMember(Order = 15)]
    public string PcpProductId { get; set; }    
}