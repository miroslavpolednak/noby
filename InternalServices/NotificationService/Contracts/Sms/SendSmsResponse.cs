﻿using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Sms;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class SendSmsResponse
{
    [ProtoMember(1)]
    public Guid NotificationId { get; set; }
}