namespace DomainServices.CodebookService.Contracts;

[DataContract]
public sealed class GenericCodebookItemWithRdmCode
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name {  get; set; }

    [DataMember(Order = 3)]
    public string RdmCode { get; set; }

    [DataMember(Order = 4)]
    public bool IsValid { get; set; }
}
