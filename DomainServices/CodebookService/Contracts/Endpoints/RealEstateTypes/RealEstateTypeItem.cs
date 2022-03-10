
namespace DomainServices.CodebookService.Contracts.Endpoints.RealEstateTypes;

[DataContract]
public class RealEstateTypeItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public bool IsValid { get; set; }

    [DataMember(Order = 4)]
    public bool IsDefault { get; set; }

    [DataMember(Order = 5)]
    public int Order { get; set; }
}