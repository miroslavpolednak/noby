using System.Text.Json.Serialization;
namespace DomainServices.CodebookService.Contracts.Endpoints.PostCodes;

[DataContract]
public class PostCodeItem
{
    [DataMember(Order = 1)]
    public string PostCode { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    public int Disctrict { get; set; }

    [DataMember(Order = 4)]
    public int Municipality { get; set; }
}