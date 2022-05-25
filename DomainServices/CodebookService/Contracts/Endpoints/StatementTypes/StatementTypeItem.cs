namespace DomainServices.CodebookService.Contracts.Endpoints.StatementTypes;

// Codebook used by RIP (avoid use attribute 'JsonIgnore')

[DataContract]
public class StatementTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public string ShortName { get; set; }

    [DataMember(Order = 4)]
    public int Order { get; set; }

    [DataMember(Order = 5)]
    public bool IsValid { get; set; }

}