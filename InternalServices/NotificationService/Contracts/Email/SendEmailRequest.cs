using CIS.Core.Validation;
using CIS.InternalServices.NotificationService.Contracts.Common;
using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Email;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class SendEmailRequest : IRequest<SendEmailResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public EmailAddress From { get; set; } = new();

    [ProtoMember(2)]
    public List<EmailAddress> To { get; set; } = new();
    
    [ProtoMember(3)]
    public List<EmailAddress> Bcc { get; set; } = new();
    
    [ProtoMember(4)]
    public List<EmailAddress> Cc { get; set; } = new();
    
    [ProtoMember(5)]
    public EmailAddress? ReplyTo { get; set; }
    
    [ProtoMember(6)]
    public string Subject { get; set; } = string.Empty;

    [ProtoMember(7)]
    public EmailContent Content { get; set; } = new();
    
    [ProtoMember(8)]
    public List<EmailAttachment> Attachments { get; set; } = new();
    
    [ProtoMember(9)]
    public Identifier Identifier { get; set; } = default!;
    
    [ProtoMember(10)]
    public string CustomId { get; set; } = string.Empty;
    
    [ProtoMember(11)]
    public string DocumentId { get; set; } = string.Empty;
}