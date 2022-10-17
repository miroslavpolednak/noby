using System.Text.Json;
using CIS.InternalServices.NotificationService.Api.Mappers;
using CIS.InternalServices.NotificationService.Api.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Msc;
using Confluent.Kafka;
using cz.kb.osbs.mcs.sender.sendapi.v2;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.Handlers;

public class SendEmailHandler : IRequestHandler<EmailSendRequest, EmailSendResponse>
{
    private readonly IProducer<Null, SendApi.v2.email.SendEmail> _mscEmailProducer;
    private readonly NotificationRepository _repository;
    private readonly ILogger<SendEmailHandler> _logger;

    public SendEmailHandler(
        IProducer<Null, SendApi.v2.email.SendEmail> mscEmailProducer,
        NotificationRepository repository,
        ILogger<SendEmailHandler> logger)
    {
        _mscEmailProducer = mscEmailProducer;
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<EmailSendResponse> Handle(EmailSendRequest request, CancellationToken cancellationToken)
    {
        var notificationResult = await _repository.CreateResult(NotificationChannel.Email, cancellationToken);
        var notificationId = notificationResult.Id.ToString();
     
        // todo: send attachments to s3
        
        // todo: mapping attachment, use s3 content
        var sendEmail = new SendApi.v2.email.SendEmail
        {
            id = notificationId,
            sender = request.From.Map(),
            to = request.To.Map().ToList(),
            bcc = request.Bcc.Map().ToList(),
            cc = request.Cc.Map().ToList(),
            replyTo = request.ReplyTo?.Map(),
            subject = request.Subject,
            content = request.Content.Map(),
            attachments = new List<Attachment>(),
        };
        
        _logger.LogInformation("Sending sms from template: {sendSms}", JsonSerializer.Serialize(sendEmail));
        
        await _mscEmailProducer.ProduceAsync(
            Topics.MscSenderIn,
            new Message<Null, SendApi.v2.email.SendEmail>
            {
                Value = sendEmail
            },
            cancellationToken);

        await _repository.UpdateResult(
            notificationResult.Id,
            NotificationState.Sent,
            new HashSet<string>(),
            cancellationToken);
        
        return new EmailSendResponse
        {
            NotificationId = notificationId
        };
    }
}