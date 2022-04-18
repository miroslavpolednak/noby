using System.Runtime.Serialization;

namespace DomainServices.UserService.Api.Dto;

[DataContract]
internal class V33PmpUser
{
    [DataMember(Order = 1)]
    public int v33id { get; set; }

    [DataMember(Order = 2)]
    public string? v33cpm { get; set; }

    [DataMember(Order = 3)]
    public string? v33icp { get; set; }

    [DataMember(Order = 4)]
    public string? v33jmeno { get; set; }

    [DataMember(Order = 5)]
    public string? v33prijmeni { get; set; }
}
