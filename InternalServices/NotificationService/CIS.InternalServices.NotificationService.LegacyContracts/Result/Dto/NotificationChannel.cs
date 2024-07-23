using System.Text.Json.Serialization;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Result.Dto;

[ProtoContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationChannel
{
    [ProtoEnum]
    Unknown = 0,
    
    [ProtoEnum]
    Sms = 1,
   
    [ProtoEnum]
    Email = 2,
}