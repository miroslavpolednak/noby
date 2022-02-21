
namespace DomainServices.CodebookService.Contracts.Endpoints.AddressTypes;

[DataContract]
public class AddressTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Code { get; set; }

    [DataMember(Order = 3)]
    public string Name { get; set; }
}

