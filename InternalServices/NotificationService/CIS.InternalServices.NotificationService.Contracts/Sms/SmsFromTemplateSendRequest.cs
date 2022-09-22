using CIS.Core.Validation;
using CIS.InternalServices.NotificationService.Contracts.Sms.Dto;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Sms;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class SmsFromTemplateSendRequest : IRequest<SmsFromTemplateSendResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public Phone Phone { get; set; } = default!;
    
    [ProtoMember(2)]
    public int ProcessingPriority { get; set; } = 1;

    [ProtoMember(3)]
    public string Type { get; set; } = string.Empty;
    
    [ProtoMember(4)]
    public List<StringKeyValuePair> Placeholders { get; set; } = default!;
}