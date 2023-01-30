using CIS.Core;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Mappers;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.S3;
using CIS.InternalServices.NotificationService.Contracts.Email;
using cz.kb.osbs.mcs.sender.sendapi.v4;
using cz.kb.osbs.mcs.sender.sendapi.v4.email;
using MediatR;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Email;

public class SendEmailFromTemplateHandler : IRequestHandler<SendEmailFromTemplateRequest, SendEmailFromTemplateResponse>
{
    private readonly IDateTime _dateTime;
    private readonly MpssEmailProducer _mpssEmailProducer;
    private readonly McsEmailProducer _mcsEmailProducer;
    private readonly NotificationRepository _repository;
    private readonly S3AdapterService _s3Service;
    private readonly S3Buckets _buckets;
    private readonly HashSet<string> _mcsSenders;
    private readonly HashSet<string> _mpssSenders;
    private readonly ILogger<SendEmailFromTemplateHandler> _logger;

    public SendEmailFromTemplateHandler(
        IDateTime dateTime,
        MpssEmailProducer mpssEmailProducer,
        McsEmailProducer mcsEmailProducer,
        NotificationRepository repository,
        S3AdapterService s3Service,
        IOptions<AppConfiguration> options,
        ILogger<SendEmailFromTemplateHandler> logger)
    {
        _dateTime = dateTime;
        _mpssEmailProducer = mpssEmailProducer;
        _mcsEmailProducer = mcsEmailProducer;
        _repository = repository;
        _s3Service = s3Service;
        _buckets = options.Value.S3Buckets;
        _mcsSenders = options.Value.EmailSenders.Mcs.Select(e => e.ToLowerInvariant()).ToHashSet();
        _mpssSenders = options.Value.EmailSenders.Mpss.Select(e => e.ToLowerInvariant()).ToHashSet();
        _logger = logger;
    }
    
    public async Task<SendEmailFromTemplateResponse> Handle(SendEmailFromTemplateRequest request, CancellationToken cancellationToken)
    {
        var attachmentKeyFilenames = new List<KeyValuePair<string, string>>();
        var domainName = request.From.Value.ToLowerInvariant().Split('@').Last();
        var bucketName = _mcsSenders.Contains(domainName)
            ? _buckets.Mcs
            : (_mpssSenders.Contains(domainName) ? _buckets.Mpss : throw new ArgumentException(domainName));
        
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
            _logger.LogError(e, $"Could not upload attachments to S3 bucket {bucketName}.");
            throw new CisServiceServerErrorException("Todo", nameof(SendEmailHandler), "SendEmail request failed due to internal server error.");
        }
     
        var result = _repository.NewEmailResult();
        result.Identity = request.Identifier?.Identity;
        result.IdentityScheme = request.Identifier?.IdentityScheme;
        result.CustomId = request.CustomId;
        result.DocumentId = request.DocumentId;
        result.RequestTimestamp = _dateTime.Now;
        
        try
        {
            await _repository.AddResult(result, cancellationToken);
            await _repository.SaveChanges(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not create EmailResult.");
            throw new CisServiceServerErrorException("Todo", nameof(SendEmailFromTemplateHandler), "SendEmailFromTemplate request failed due to internal server error.");
        }
        
        // todo:
        // var sendEmail = new SendEmail
        // {
            // id = result.Id.ToString(),
            // sender = request.From.Map(),
            // to = request.To.Map().ToList(),
            // bcc = request.Bcc.Map().ToList(),
            // cc = request.Cc.Map().ToList(),
            // replyTo = request.ReplyTo?.Map(),
            // subject = request.Subject,
            // content = request.Content.Map(),
            // attachments = attachments,
            // notificationConsumer = new NotificationConsumer
            // {
            //     consumerId = "HF"
            // }
        // };
        
        try
        {
            if (_mcsSenders.Contains(domainName))
            {
                // await _mcsEmailProducer.SendEmail(sendEmail, cancellationToken);
            }
            else if (_mpssSenders.Contains(domainName))
            {
                // await _mpssEmailProducer.SendEmail(sendEmail, cancellationToken);
            }
            else
            {
                throw new ArgumentException(domainName);
            }
        }
        catch(Exception e)
        {
            _logger.LogError(e, "Could not produce message SendEmail to KAFKA.");
            _repository.DeleteResult(result);
            await _repository.SaveChanges(cancellationToken);
            throw new CisServiceServerErrorException("Todo", nameof(SendEmailFromTemplateHandler), "SendEmailFromTemplate request failed due to internal server error.");
        }
        
        return new SendEmailFromTemplateResponse { NotificationId = result.Id };
    }
}