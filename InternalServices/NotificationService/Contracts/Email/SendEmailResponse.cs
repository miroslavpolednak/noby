﻿using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Email;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class SendEmailResponse
{
    [ProtoMember(1)]
    public Guid NotificationId { get; set; }
}