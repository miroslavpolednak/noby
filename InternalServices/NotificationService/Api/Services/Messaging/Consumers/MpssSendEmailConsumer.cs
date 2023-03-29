using CIS.InternalServices.NotificationService.Api.Handlers.Email.Models;
using CIS.InternalServices.NotificationService.Api.Handlers.Email.Requests;
using MassTransit;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Services.Messaging.Consumers;

public class MpssSendEmailConsumer : IConsumer<MpssSendApi.v1.email.SendEmail>
{
    private readonly IMediator _mediator;

    public MpssSendEmailConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<MpssSendApi.v1.email.SendEmail> context)
    {
        var sendEmail = context.Message;

        if (Guid.TryParse(sendEmail.id, out var id))
        {
            var request = new SendEmailConsumeRequest
            {
                Id = id,
                From = sendEmail.sender.value,
                ReplyTo = sendEmail.replyTo?.value ?? string.Empty,
                Subject = sendEmail.subject,
                Content = sendEmail.content?.text ?? string.Empty,
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