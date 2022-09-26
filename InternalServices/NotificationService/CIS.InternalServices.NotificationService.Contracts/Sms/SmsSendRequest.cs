using CIS.Core.Validation;
using CIS.InternalServices.NotificationService.Contracts.Sms.Dto;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Sms;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class SmsSendRequest : IRequest<SmsSendResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public Phone Phone { get; set; } = default!;
    
    [ProtoMember(2)]
    public int ProcessingPriority { get; set; } = 1;

    [ProtoMember(3)]
    public SmsNotificationType Type { get; set; }
    
    [ProtoMember(4)]
    public string Text { get; set; } = string.Empty;
}