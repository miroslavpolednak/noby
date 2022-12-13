using CIS.InternalServices.NotificationService.Api.Handlers.Email.Models;
using CIS.InternalServices.NotificationService.Api.Handlers.Email.Requests;
using cz.kb.osbs.mcs.sender.sendapi.v4.email;
using MassTransit;
using MassTransit.Mediator;

namespace CIS.InternalServices.NotificationService.Api.Services.Mcs.Consumers;

// todo: change Mcs.SendEmail for Mpss.SendEmail
public class SendEmailConsumer : IConsumer<SendEmail>
{
    private readonly IMediator _mediator;

    public SendEmailConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<SendEmail> context)
    {
        var sendEmail = context.Message;
        
        if (Guid.TryParse(sendEmail.id, out var id))
        {
            var request = new SendEmailConsumeRequest
            {
                Id = id,
                From = sendEmail.sender.value,
                ReplyTo = sendEmail.replyTo.value,
                Subject = sendEmail.subject,
                Content = sendEmail.content.text,
                To = sendEmail.to.Select(t => t.value).ToList(),
                Cc = sendEmail.cc.Select(t => t.value).ToList(),
                Bcc = sendEmail.bcc.Select(t => t.value).ToList(),
                Attachments = sendEmail.attachments.Select(a => new SendEmailAttachment
                {
                    S3Key = a.s3Content.objectKey,
                    Filename = a.s3Content.filename
                }).ToList()
            };
            await _mediator.Send(request);   
        }
    }
}