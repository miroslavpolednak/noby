
using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.Genders;

[DataContract]
public class GenderItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Code { get; set; }

    [DataMember(Order = 3)]
    public string Name { get; set; }

    [DataMember(Order = 4)]
    [JsonIgnore]
    public string RDMCode { get; set; }
}

