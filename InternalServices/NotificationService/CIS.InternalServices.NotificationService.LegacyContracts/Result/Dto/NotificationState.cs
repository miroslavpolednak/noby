﻿using System.Text.Json.Serialization;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.LegacyContracts.Result.Dto;

[ProtoContract]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationState
{
    [ProtoEnum]
    Unknown = 0,
    
    [ProtoEnum]
    InProgress = 1,
    
    [ProtoEnum]
    Unsent = 2,
   
    [ProtoEnum]
    Sent = 3,
    
    [ProtoEnum]
    Delivered = 4,
    
    [ProtoEnum]
    Invalid = 5,
    
    [ProtoEnum]
    Error = 6
}