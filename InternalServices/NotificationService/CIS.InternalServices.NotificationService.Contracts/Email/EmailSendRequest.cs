using CIS.Core.Validation;
using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Email;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class EmailSendRequest : IRequest<EmailSendResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public string From { get; set; } = string.Empty;

    [ProtoMember(2)]
    public List<string> To { get; set; } = new();
    
    [ProtoMember(3)]
    public List<string> Bcc { get; set; } = new();
    
    [ProtoMember(4)]
    public List<string> Cc { get; set; } = new();
    
    [ProtoMember(5)]
    public string? ReplyTo { get; set; }
    
    [ProtoMember(6)]
    public string Subject { get; set; } = string.Empty;

    [ProtoMember(7)]
    public EmailContent Content { get; set; } = new();
    
    [ProtoMember(8)]
    public List<EmailAttachment> Attachments { get; set; } = new();
}