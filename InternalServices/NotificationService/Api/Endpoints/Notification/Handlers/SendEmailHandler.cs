using System.Text.Json;
using CIS.InternalServices.NotificationService.Api.Mcs.Mappers;
using CIS.InternalServices.NotificationService.Api.Mcs.Producers;
using CIS.InternalServices.NotificationService.Api.Repositories;
using CIS.InternalServices.NotificationService.Api.S3;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using cz.kb.osbs.mcs.sender.sendapi.v2;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.Handlers;

public class SendEmailHandler : IRequestHandler<EmailSendRequest, EmailSendResponse>
{
    private readonly BusinessEmailProducer _businessEmailProducer;
    private readonly LogmanEmailProducer _logmanEmailProducer;
    private readonly NotificationRepository _repository;
    private readonly S3AdapterService _s3Service;
    private readonly ILogger<SendEmailHandler> _logger;

    public SendEmailHandler(
        BusinessEmailProducer businessEmailProducer,
        LogmanEmailProducer logmanEmailProducer,
        NotificationRepository repository,
        S3AdapterService s3Service,
        ILogger<SendEmailHandler> logger)
    {
        _businessEmailProducer = businessEmailProducer;
        _logmanEmailProducer = logmanEmailProducer;
        _repository = repository;
        _s3Service = s3Service;
        _logger = logger;
    }
    
    public async Task<EmailSendResponse> Handle(EmailSendRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        var notificationResult = await _repository.CreateResult(NotificationChannel.Email, cancellationToken);
        var notificationId = notificationResult.Id;
        
        var attachments = new List<Attachment>();

        try
        {
            foreach (var attachment in request.Attachments)
            {
                // todo: Buckets.Mcs or Buckets.Mpss
                var bucketName = Buckets.Mcs;
                var objectKey = await _s3Service.UploadFile(attachment.Binary, bucketName);
                attachments.Add(new Attachment
                {
                    s3Content = new S3Content
                    {
                        filename = attachment.Filename,
                        objectKey = objectKey
                    }
                });
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not upload attachments.");
            throw;
        }

        var sendEmail = new SendApi.v2.email.SendEmail
        {
            id = notificationId.ToString(),
            sender = request.From.Map(),
            to = request.To.Map().ToList(),
            bcc = request.Bcc.Map().ToList(),
            cc = request.Cc.Map().ToList(),
            replyTo = request.ReplyTo?.Map(),
            subject = request.Subject,
            content = request.Content.Map(),
            attachments = attachments
        };
        
        _logger.LogInformation("Sending email: {sendEmail}", JsonSerializer.Serialize(sendEmail));

        // todo: decide Logman or Business
        var sendResult = await _logmanEmailProducer.SendEmail(sendEmail, cancellationToken);
        
        var updateResult = await _repository.UpdateResult(
            notificationId,
            NotificationState.Sent,
            new HashSet<string>(),
            cancellationToken);
        
        return new EmailSendResponse
        {
            NotificationId = notificationId
        };
    }
}