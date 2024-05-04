using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Result.Dto;

[ProtoContract]
public class RequestData
{
    [ProtoMember(1)]
    public SmsData? SmsData { get; set; }
    
    [ProtoMember(2)]
    public EmailData? EmailData { get; set; }
}