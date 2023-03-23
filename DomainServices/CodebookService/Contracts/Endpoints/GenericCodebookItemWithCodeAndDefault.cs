namespace DomainServices.CodebookService.Contracts.Endpoints;

[DataContract]
public sealed class GenericCodebookItemWithCodeAndDefault
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public string Code { get; set; }

    [DataMember(Order = 4)]
    public bool IsValid { get; set; }

    [DataMember(Order = 5)]
    public bool IsDefault { get; set; }
}
