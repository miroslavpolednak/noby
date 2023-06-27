using CIS.Core;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Mappers;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.S3.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.Email;
using MediatR;
using Microsoft.Extensions.Options;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Email;

public class SendEmailHandler : IRequestHandler<SendEmailRequest, SendEmailResponse>
{
    private readonly IDateTime _dateTime;
    private readonly IMpssEmailProducer _mpssEmailProducer;
    private readonly IMcsEmailProducer _mcsEmailProducer;
    private readonly IUserAdapterService _userAdapterService;
    private readonly INotificationRepository _repository;
    private readonly IS3AdapterService _s3Service;
    private readonly S3Buckets _buckets;
    private readonly HashSet<string> _mcsSenders;
    private readonly HashSet<string> _mpssSenders;
    private readonly ILogger<SendEmailHandler> _logger;

    public SendEmailHandler(
        IDateTime dateTime,
        IMpssEmailProducer mpssEmailProducer,
        IMcsEmailProducer mcsEmailProducer,
        IUserAdapterService userAdapterService,
        INotificationRepository repository,
        IS3AdapterService s3Service,
        IOptions<AppConfiguration> options,
        ILogger<SendEmailHandler> logger)
    {
        _dateTime = dateTime;
        _mpssEmailProducer = mpssEmailProducer;
        _mcsEmailProducer = mcsEmailProducer;
        _userAdapterService = userAdapterService;
        _repository = repository;
        _s3Service = s3Service;
        _buckets = options.Value.S3Buckets;
        _mcsSenders = options.Value.EmailSenders.Mcs.Select(e => e.ToLowerInvariant()).ToHashSet();
        _mpssSenders = options.Value.EmailSenders.Mpss.Select(e => e.ToLowerInvariant()).ToHashSet();
        _logger = logger;
    }
    
    public async Task<SendEmailResponse> Handle(SendEmailRequest request, CancellationToken cancellationToken)
    {
        var username = _userAdapterService
            .CheckSendEmailAccess()
            .GetUsername();
        
        var attachmentKeyFilenames = new List<KeyValuePair<string, string>>();
        var domainName = request.From.Value.ToLowerInvariant().Split('@').Last();
        var bucketName = _mcsSenders.Contains(domainName)
            ? _buckets.Mcs
            : (_mpssSenders.Contains(domainName) ? _buckets.Mpss : throw new ArgumentException(domainName));
        
        try
        {
            foreach (var attachment in request.Attachments)
            {
                var content = Convert.FromBase64String(attachment.Binary);
                var objectKey = await _s3Service.UploadFile(content, bucketName, cancellationToken);
                attachmentKeyFilenames.Add(new (objectKey, attachment.Filename));
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not upload attachments to S3 bucket {bucketName}.");
            throw new CisServiceServerErrorException(ErrorHandling.ErrorCodeMapper.UploadAttachmentFailed, nameof(SendEmailHandler), "SendEmail request failed due to internal server error.");
        }

        var result = _repository.NewEmailResult();
        result.Identity = request.Identifier?.Identity;
        result.IdentityScheme = request.Identifier?.IdentityScheme;
        result.CustomId = request.CustomId;
        result.DocumentId = request.DocumentId;
        result.RequestTimestamp = _dateTime.Now;

        result.CreatedBy = username;
        
        try
        {
            await _repository.AddResult(result, cancellationToken);
            await _repository.SaveChanges(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not create EmailResult.");
            throw new CisServiceServerErrorException(ErrorHandling.ErrorCodeMapper.CreateEmailResultFailed, nameof(SendEmailHandler), "SendEmail request failed due to internal server error.");
        }

        try
        {
            var consumerId = _userAdapterService.GetConsumerId();
            
            if (_mcsSenders.Contains(domainName))
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
            }
            else if (_mpssSenders.Contains(domainName))
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
            else
            {
                throw new ArgumentException(domainName);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not produce message SendEmail to KAFKA.");
            _repository.DeleteResult(result);
            await _repository.SaveChanges(cancellationToken);
            throw new CisServiceServerErrorException(ErrorHandling.ErrorCodeMapper.ProduceSendEmailError, nameof(SendEmailHandler), "SendEmail request failed due to internal server error.");
        }

        return new SendEmailResponse { NotificationId = result.Id };
    }
}