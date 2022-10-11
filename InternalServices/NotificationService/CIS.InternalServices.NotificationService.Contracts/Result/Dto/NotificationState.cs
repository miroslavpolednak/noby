using System.Text.Json.Serialization;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result.Dto;

[ProtoContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationState
{
    [ProtoEnum]
    Unknown = 0,
    
    [ProtoEnum]
    Unsent = 1,
   
    [ProtoEnum]
    Sent = 2,
    
    [ProtoEnum]
    Delivered = 3,
    
    [ProtoEnum]
    Invalid = 4,
}