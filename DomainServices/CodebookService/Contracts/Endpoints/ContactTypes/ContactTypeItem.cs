
namespace DomainServices.CodebookService.Contracts.Endpoints.ContactTypes;

[DataContract]
public class ContactTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Code { get; set; }

    [DataMember(Order = 3)]
    public string Name { get; set; }

    [DataMember(Order = 4)]
    public string CodeKb { get; set; }
}
