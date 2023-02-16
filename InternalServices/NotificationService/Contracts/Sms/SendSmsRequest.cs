using CIS.Core.Validation;
using CIS.InternalServices.NotificationService.Contracts.Common;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Sms;

[ProtoContract]
public class SendSmsRequest : IRequest<SendSmsResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public string PhoneNumber { get; set; } = default!;
    
    [ProtoMember(2)]
    public int? ProcessingPriority { get; set; }

    [ProtoMember(3)]
    public string Type { get; set; } = string.Empty;
    
    [ProtoMember(4)]
    public string Text { get; set; } = string.Empty;

    [ProtoMember(5)]
    public Identifier? Identifier { get; set; }
    
    [ProtoMember(6)]
    public string? CustomId { get; set; }
    
    [ProtoMember(7)]
    public string? DocumentId { get; set; }
}