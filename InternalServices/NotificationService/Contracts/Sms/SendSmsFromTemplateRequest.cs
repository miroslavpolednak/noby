﻿using CIS.Core.Validation;
using CIS.InternalServices.NotificationService.Contracts.Common;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Sms;

[ProtoContract]
public class SendSmsFromTemplateRequest : IRequest<SendSmsFromTemplateResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [ProtoMember(2)]
    public int ProcessingPriority { get; set; } = 1;

    [ProtoMember(3)]
    public string Type { get; set; } = string.Empty;
    
    [ProtoMember(4)]
    public List<StringKeyValuePair> Placeholders { get; set; } = new();
    
    [ProtoMember(5)]
    public Identifier? Identifier { get; set; }
    
    [ProtoMember(6)]
    public string? CustomId { get; set; }
    
    [ProtoMember(7)]
    public string? DocumentId { get; set; }
}