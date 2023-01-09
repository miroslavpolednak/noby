namespace DomainServices.CodebookService.Contracts.Endpoints.LoanKinds;

[DataContract]
public class LoanKindsItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public int? MandantId { get; set; }

    [DataMember(Order = 3)]
    public string Name { get; set; }

    [DataMember(Order = 4)]
    public bool IsDefault { get; set; }
    
    [DataMember(Order = 5)]
    public bool IsValid { get; set; }
}
