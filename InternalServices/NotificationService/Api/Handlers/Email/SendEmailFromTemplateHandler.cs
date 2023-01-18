using CIS.Core;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Mappers;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.S3;
using CIS.InternalServices.NotificationService.Contracts.Email;
using cz.kb.osbs.mcs.sender.sendapi.v4;
using cz.kb.osbs.mcs.sender.sendapi.v4.email;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Email;

public class SendEmailFromTemplateHandler : IRequestHandler<SendEmailFromTemplateRequest, SendEmailFromTemplateResponse>
{
    private readonly IDateTime _dateTime;
    private readonly MpssEmailProducer _mpssEmailProducer;
    private readonly McsEmailProducer _mcsEmailProducer;
    private readonly NotificationRepository _repository;
    private readonly S3AdapterService _s3Service;
    private readonly ILogger<SendEmailFromTemplateHandler> _logger;

    public SendEmailFromTemplateHandler(
        IDateTime dateTime,
        MpssEmailProducer mpssEmailProducer,
        McsEmailProducer mcsEmailProducer,
        NotificationRepository repository,
        S3AdapterService s3Service,
        ILogger<SendEmailFromTemplateHandler> logger)
    {
        _dateTime = dateTime;
        _mpssEmailProducer = mpssEmailProducer;
        _mcsEmailProducer = mcsEmailProducer;
        _repository = repository;
        _s3Service = s3Service;
        _logger = logger;
    }
    
    public async Task<SendEmailFromTemplateResponse> Handle(SendEmailFromTemplateRequest request, CancellationToken cancellationToken)
    {
        var attachmentKeyFilenames = new List<KeyValuePair<string, string>>();
        var host = request.From.Value.ToLowerInvariant().Split('@').Last();
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
            throw new CisServiceUnavailableException("Todo", nameof(SendEmailFromTemplateHandler), "Todo");
        }
     
        var result = _repository.NewEmailResult();
        result.Identity = request.Identifier?.Identity;
        result.IdentityScheme = request.Identifier?.IdentityScheme;
        result.CustomId = request.CustomId;
        result.DocumentId = request.DocumentId;
        result.RequestTimestamp = _dateTime.Now;
        
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
            // attachments = attachments
        // };
        
        try
        {
            if (host == "kb.cz")
            {
                // await _mcsEmailProducer.SendEmail(sendEmail, cancellationToken);
                result.HandoverToMcsTimestamp = _dateTime.Now;
            }
            else
            {
                // await _mpssEmailProducer.SendEmail(sendEmail, cancellationToken);
            }
            
            await _repository.AddResult(result, cancellationToken);
            await _repository.SaveChanges(cancellationToken);
        }
        catch(Exception e)
        {
            _logger.LogError(e, $"Could not produce message {nameof(SendEmail)} to KAFKA.");
            throw new CisServiceUnavailableException("Todo", nameof(SendEmailFromTemplateHandler), "Todo");
        }
        
        return new SendEmailFromTemplateResponse
        {
            NotificationId = Guid.NewGuid()
        };
    }
}