using CIS.Core.Validation;
using CIS.InternalServices.NotificationService.Contracts.Common;
using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Email;

[ProtoContract]
public class SendEmailFromTemplateRequest : IRequest<SendEmailFromTemplateResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public EmailAddress From { get; set; } = new();
    
    [ProtoMember(2)]
    public List<EmailAttachment> Attachments { get; set; } = new();
    
    // todo: rest fields
    
    [ProtoMember(3)]
    public Identifier? Identifier { get; set; }
    
    [ProtoMember(4)]
    public string? CustomId { get; set; }
    
    [ProtoMember(5)]
    public string? DocumentId { get; set; }
}