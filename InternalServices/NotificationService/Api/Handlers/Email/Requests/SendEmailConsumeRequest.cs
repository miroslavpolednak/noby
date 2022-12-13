using CIS.InternalServices.NotificationService.Api.Handlers.Email.Models;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Email.Requests;

public class SendEmailConsumeRequest : IRequest<SendEmailConsumeResponse>
{
    public Guid Id { get; set; }
    public string From { get; set; } = null!;
    public string ReplyTo { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Content { get; set; } = null!;
    public List<string> To { get; set; } = null!;
    public List<string> Cc { get; set; } = null!;
    public List<string> Bcc { get; set; } = null!;
    public List<SendEmailAttachment> Attachments { get; set; } = null!;
}