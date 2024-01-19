using ProtoBuf;
using System.Text.Json.Serialization;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Statistics.Dto;

[ProtoContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SenderType : byte
{
    [ProtoEnum]
    Unknown = 0, 
    
    [ProtoEnum]
    KB = 1,

    [ProtoEnum]
    MP = 2
}
