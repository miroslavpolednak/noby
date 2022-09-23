using System.Text.Json.Serialization;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Sms.Dto;

[ProtoContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SmsNotificationType
{
    [ProtoEnum]
    Unknown = 0,
    
    // todo:
}