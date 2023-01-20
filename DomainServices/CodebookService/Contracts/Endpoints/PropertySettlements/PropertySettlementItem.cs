namespace DomainServices.CodebookService.Contracts.Endpoints.PropertySettlements;

[DataContract]
public class PropertySettlementItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public string NameEnglish { get; set; }

    [DataMember(Order = 4)]
    public bool IsValid { get; set; }

    [DataMember(Order = 5)]
    public List<int> MaritalStateIds { get; set; }

    [DataMember(Order = 6)]
    public int? Order { get; set; }
}