using CIS.Core;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Mappers;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Infrastructure;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.S3;
using CIS.InternalServices.NotificationService.Contracts.Email;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Email;

public class SendEmailHandler : IRequestHandler<SendEmailRequest, SendEmailResponse>
{
    private readonly IDateTime _dateTime;
    private readonly MpssEmailProducer _mpssEmailProducer;
    private readonly McsEmailProducer _mcsEmailProducer;
    private readonly NotificationRepository _repository;
    private readonly S3AdapterService _s3Service;
    private readonly ILogger<SendEmailHandler> _logger;

    public SendEmailHandler(
        IDateTime dateTime,
        MpssEmailProducer mpssEmailProducer,
        McsEmailProducer mcsEmailProducer,
        NotificationRepository repository,
        S3AdapterService s3Service,
        ILogger<SendEmailHandler> logger)
    {
        _dateTime = dateTime;
        _mpssEmailProducer = mpssEmailProducer;
        _mcsEmailProducer = mcsEmailProducer;
        _repository = repository;
        _s3Service = s3Service;
        _logger = logger;
    }
    
    public async Task<SendEmailResponse> Handle(SendEmailRequest request, CancellationToken cancellationToken)
    {
        var attachmentKeyFilenames = new List<KeyValuePair<string, string>>();
        var host = request.From.Value.ToLowerInvariant().Split('@').Last();
        
        // todo: Buckets to configuration
        var bucketName = host == "kb.cz" ? Buckets.Mcs : Buckets.Mpss;
        
        try
        {
            foreach (var attachment in request.Attachments)
            {
                var objectKey = await _s3Service.UploadFile(attachment.Binary, bucketName);
                attachmentKeyFilenames.Add(new (objectKey, attachment.Filename));
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not upload attachments.");
            throw new CisServiceUnavailableException("Todo", nameof(SendEmailHandler), "Todo");
        }

        var result = _repository.NewEmailResult();
        result.Identity = request.Identifier?.Identity;
        result.IdentityScheme = request.Identifier?.IdentityScheme;
        result.CustomId = request.CustomId;
        result.DocumentId = request.DocumentId;
        result.RequestTimestamp = _dateTime.Now;
        
        try
        {
            // todo: map user to consumerId
            var consumerId = NotificationConsumerIds.Starbuild;
            
            if (host == "kb.cz")
            {
                var sendEmail = new McsSendApi.v4.email.SendEmail
                {
                    id = result.Id.ToString(),
                    notificationConsumer = McsEmailMappers.MapToMcs(consumerId),
                    sender = request.From.MapToMcs(),
                    to = request.To.MapToMcs().ToList(),
                    bcc = request.Bcc.MapToMcs().ToList(),
                    cc = request.Cc.MapToMcs().ToList(),
                    replyTo = request.ReplyTo?.MapToMcs(),
                    subject = request.Subject,
                    content = request.Content.MapToMcs(),
                    attachments = attachmentKeyFilenames
                        .Select(kv => McsEmailMappers.MapToMcs(kv.Key, kv.Value))
                        .ToList()
                };
                
                await _mcsEmailProducer.SendEmail(sendEmail, cancellationToken);
                result.HandoverToMcsTimestamp = _dateTime.Now;
            }
            else
            {
                var sendEmail = new MpssSendApi.v1.email.SendEmail
                {
                    id = result.Id.ToString(),
                    notificationConsumer = MpssEmailMappers.MapToMpss(consumerId),
                    sender = request.From.MapToMpss(),
                    to = request.To.MapToMpss().ToList(),
                    bcc = request.Bcc.MapToMpss().ToList(),
                    cc = request.Cc.MapToMpss().ToList(),
                    replyTo = request.ReplyTo?.MapToMpss(),
                    subject = request.Subject,
                    content = request.Content.MapToMpss(),
                    attachments = attachmentKeyFilenames
                        .Select(kv => MpssEmailMappers.MapToMpss(kv.Key, kv.Value))
                        .ToList()
                };
                
                await _mpssEmailProducer.SendEmail(sendEmail, cancellationToken);
            }
            
            await _repository.AddResult(result, cancellationToken);
            await _repository.SaveChanges(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not produce message SendEmail to KAFKA.");
            throw new CisServiceUnavailableException("Todo", nameof(SendEmailHandler), "Todo");
        }

        return new SendEmailResponse
        {
            NotificationId = result.Id
        };
    }
}