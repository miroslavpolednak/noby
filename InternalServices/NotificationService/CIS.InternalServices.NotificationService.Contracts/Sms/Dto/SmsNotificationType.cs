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
    [ProtoEnum]
    StarBuild = 1,
    
    [ProtoEnum]
    Noby = 2,
}