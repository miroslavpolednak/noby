using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Common;

[ProtoContract]
public sealed class Phone
{
    [ProtoMember(1)]
    public string CountryCode { get; set; } = string.Empty;
    
    [ProtoMember(2)]
    public string NationalNumber { get; set; } = string.Empty;
}