using CIS.Core.Validation;
using CIS.InternalServices.NotificationService.Contracts.Email.Dto;
using MediatR;
using ProtoBuf;

namespace CIS.InternalServices.NotificationService.Contracts.Email;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class EmailFromTemplateSendRequest : IRequest<EmailFromTemplateSendResponse>, IValidatableRequest
{
    [ProtoMember(1)]
    public EmailAddress From { get; set; } = new();
    
    [ProtoMember(2)]
    public List<EmailAttachment> Attachments { get; set; } = new();
    
    // todo: rest fields
}