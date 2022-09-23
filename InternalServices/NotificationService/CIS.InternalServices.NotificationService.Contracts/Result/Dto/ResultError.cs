﻿using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Result.Dto;

[ProtoContract]
public class ResultError
{
    [ProtoMember(1)]
    public string Code { get; set; }
    
    [ProtoMember(2)]
    public string Message { get; set; }
}