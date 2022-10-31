using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.ContactTypes;

[DataContract]
public class ContactTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public string MpDigiApiCode { get; set; }

    [DataMember(Order = 4)]
    [JsonIgnore]
    public bool IsValid { get; set; }

    [DataMember(Order = 5)]
    public int MandantId { get; set; }

}
