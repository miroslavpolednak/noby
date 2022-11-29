using CIS.InternalServices.NotificationService.Api.Handlers.Email.Requests;
using CIS.InternalServices.NotificationService.Api.Services.S3;
using CIS.InternalServices.NotificationService.Api.Services.Smtp;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Email;

public class SendEmailConsumeHandler : IRequestHandler<SendEmailConsumeRequest, SendEmailConsumeResponse>
{
    private readonly SmtpAdapterService _smtpAdapterService;
    private readonly S3AdapterService _s3AdapterService;

    public SendEmailConsumeHandler(
        SmtpAdapterService smtpAdapterService,
        S3AdapterService s3AdapterService)
    {
        _smtpAdapterService = smtpAdapterService;
        _s3AdapterService = s3AdapterService;
    }
    
    public async Task<SendEmailConsumeResponse> Handle(SendEmailConsumeRequest request, CancellationToken cancellationToken)
    {
        foreach (var attachmentKey in request.AttachmentKeys)
        {
            // todo:
        }
        
        await _smtpAdapterService.SendEmail(
            request.From,
            request.ReplyTo,
            request.Subject,
            request.Content,
            request.To,
            request.Cc,
            request.Bcc,
            new List<KeyValuePair<string, byte[]>>()
            );
        
        return new SendEmailConsumeResponse();
    }
}