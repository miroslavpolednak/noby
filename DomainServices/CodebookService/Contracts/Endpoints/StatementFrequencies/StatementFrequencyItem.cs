namespace DomainServices.CodebookService.Contracts.Endpoints.StatementFrequencies;

[DataContract]
public sealed class StatementFrequencyItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string FrequencyCode { get; set; }

    [DataMember(Order = 3)]
    public int FrequencyValue { get; set; }

    [DataMember(Order = 4)]
    public int Order { get; set; }

    [DataMember(Order = 5)]
    public string Name { get; set; }

    [DataMember(Order = 6)]
    public bool IsValid { get; set; }

    [DataMember(Order = 7)]
    public bool IsDefault { get; set; }
}
