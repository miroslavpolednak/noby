﻿using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Statistics.Dto;

[ProtoContract]
public class Statistics
{
    [ProtoMember(1)]
    public Status? Email { get; set; }

    [ProtoMember(2)]
    public Status? SMS { get; set; }
}
