﻿using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Sms.Dto;

[ProtoContract]
public class StringKeyValuePair
{
    [ProtoMember(1)]
    public string Key { get; set; } = string.Empty;
    
    [ProtoMember(2)]
    public string Value { get; set; } = string.Empty;  
}