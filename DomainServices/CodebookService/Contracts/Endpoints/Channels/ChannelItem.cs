namespace DomainServices.CodebookService.Contracts.Endpoints.Channels;

[DataContract]
public class ChannelItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public int? MandantId { get; set; }

    [DataMember(Order = 3)]
    public string Name { get; set; }

    [DataMember(Order = 4)]
    public string Code { get; set; }

    [DataMember(Order = 5)]
    public bool IsValid { get; set; }
}
