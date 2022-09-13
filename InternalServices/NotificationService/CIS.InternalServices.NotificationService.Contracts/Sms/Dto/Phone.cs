using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Sms.Dto;

[ProtoContract]
public sealed class Phone
{
    [ProtoMember(1)]
    public string CountryCode { get; set; } = string.Empty;
    
    [ProtoMember(2)]
    public string NationalPhoneNumber { get; set; } = string.Empty;
}