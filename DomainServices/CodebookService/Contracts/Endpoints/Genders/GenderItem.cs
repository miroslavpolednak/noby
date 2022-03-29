using System.Text.Json.Serialization;

namespace DomainServices.CodebookService.Contracts.Endpoints.Genders;

[DataContract]
public class GenderItem
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string Name { get; set; }

    [DataMember(Order = 3)]
    [JsonIgnore]
    public CIS.Foms.Enums.Genders MpHomeCode { get; set; }
    
    [DataMember(Order = 4)]
    [JsonIgnore]
    public int KonsDBCode { get; set; }

    [DataMember(Order = 5)]
    [JsonIgnore]
    public string KbCmCode { get; set; }

    [DataMember(Order = 6)]
    [JsonIgnore]
    public string StarBuildJsonCode { get; set; }
}