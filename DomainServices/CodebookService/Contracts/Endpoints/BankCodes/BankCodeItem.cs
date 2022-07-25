using System.Text.Json.Serialization;
namespace DomainServices.CodebookService.Contracts.Endpoints.BankCodes;

[DataContract]
public class BankCodeItem
{
    [DataMember(Order = 1)]
    public string BankCode { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public string ShortName { get; set; }

    [DataMember(Order = 4)]
    public string State { get; set; }

    [DataMember(Order = 5)]
    [JsonIgnore]
    public bool IsValid { get; set; }
}
