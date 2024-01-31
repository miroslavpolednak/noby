using CIS.Core;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Database.DocumentDataEntities;
using CIS.InternalServices.NotificationService.Api.Legacy;
using CIS.InternalServices.NotificationService.Api.Messaging.Mappers;
using CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.S3.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.LegacyContracts.Email;
using DomainServices.CodebookService.Clients;
using Microsoft.Extensions.Options;
using SharedComponents.DocumentDataStorage;
using SharedComponents.Storage;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Email;

internal sealed class SendEmailHandler : IRequestHandler<SendEmailRequest, SendEmailResponse>
{
    private readonly TimeProvider _dateTime;
    private readonly IMcsEmailProducer _mcsEmailProducer;
    private readonly IUserAdapterService _userAdapterService;
    private readonly INotificationRepository _repository;
    private readonly ICodebookServiceClient _codebookService;
    private readonly IS3AdapterService _s3Service;
    private readonly IOptions<SharedComponents.Storage.Configuration.StorageConfiguration> _storageConfiguration;
    private readonly HashSet<string> _mcsSenders;
    private readonly HashSet<string> _mpssSenders;
    private readonly ILogger<SendEmailHandler> _logger; 
    private readonly IDocumentDataStorage _documentDataStorage;

    public SendEmailHandler(
        TimeProvider dateTime,
        IMcsEmailProducer mcsEmailProducer,
        IUserAdapterService userAdapterService,
        INotificationRepository repository,
        ICodebookServiceClient codebookService,
        IS3AdapterService s3Service,
        IOptions<AppConfiguration> options,
        ILogger<SendEmailHandler> logger,
        IDocumentDataStorage documentDataStorage,
        IOptions<SharedComponents.Storage.Configuration.StorageConfiguration> storageConfiguration)
    {
        _dateTime = dateTime;
        _mcsEmailProducer = mcsEmailProducer;
        _userAdapterService = userAdapterService;
        _repository = repository;
        _codebookService = codebookService;
        _s3Service = s3Service;
        _mcsSenders = options.Value.EmailSenders.Mcs.Select(e => e.ToLowerInvariant()).ToHashSet();
        _mpssSenders = options.Value.EmailSenders.Mpss.Select(e => e.ToLowerInvariant()).ToHashSet();
        _logger = logger;
        _documentDataStorage = documentDataStorage;
        _storageConfiguration = storageConfiguration;
    }

    public async Task<SendEmailResponse> Handle(SendEmailRequest request, CancellationToken cancellationToken)
    {
        var username = _userAdapterService
            .CheckSendEmailAccess()
            .GetUsername();
        
        if (!HashAlgorithms.Algorithms.Contains(request.DocumentHash?.HashAlgorithm ?? ""))
        {
            throw new CisValidationException($"Invalid HashAlgorithm = '{request.DocumentHash?.HashAlgorithm}'.");
        }
        
        var attachmentKeyFilenames = new List<KeyValuePair<string, string>>();
        var domainName = request.From.Value.ToLowerInvariant().Split('@').Last();

        var result = _repository.NewEmailResult();
        result.Identity = request.Identifier?.Identity;
        result.IdentityScheme = request.Identifier?.IdentityScheme;
        result.CaseId = request.CaseId;
        result.CustomId = request.CustomId;
        result.DocumentId = request.DocumentId;
        result.DocumentHash = request.DocumentHash?.Hash;
        result.HashAlgorithm = request.DocumentHash?.HashAlgorithm;
        result.RequestTimestamp = _dateTime.GetLocalNow().DateTime;
        result.SenderType = _mcsSenders.Contains(domainName) ? LegacyContracts.Statistics.Dto.SenderType.KB
            : _mpssSenders.Contains(domainName) ? LegacyContracts.Statistics.Dto.SenderType.MP
            : throw new ArgumentException(domainName);

        result.CreatedBy = username;
        
        try
        {
            await _repository.AddResult(result, cancellationToken);
            await _repository.SaveChanges(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not create EmailResult.");
            throw new CisServiceServerErrorException(ErrorCodeMapper.CreateEmailResultFailed, nameof(SendEmailHandler), "SendEmail request failed due to internal server error.");
        }

        try
        {
            var consumerId = _userAdapterService.GetConsumerId();
            
            if (result.SenderType == LegacyContracts.Statistics.Dto.SenderType.KB)
            {
                try
                {
                    foreach (var attachment in request.Attachments)
                    {
                        string bucket = _storageConfiguration.Value.StorageClients[nameof(IMcsStorage)].AmazonS3.Bucket;
                        var content = Convert.FromBase64String(attachment.Binary);
                        var objectKey = await _s3Service.UploadFile(content, bucket, cancellationToken);
                        attachmentKeyFilenames.Add(new(objectKey, attachment.Filename));
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Could not upload attachments to S3 bucket.");
                    throw new CisServiceServerErrorException(ErrorCodeMapper.UploadAttachmentFailed, nameof(SendEmailHandler), "SendEmail request failed due to internal server error.");
                }

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
            else if (result.SenderType == LegacyContracts.Statistics.Dto.SenderType.MP)
            {
                SendEmail email = new()
                {
                    Data = request
                };

                await _documentDataStorage.Add(result.Id.ToString(), email, cancellationToken);

            }
            else
            {
                throw new ArgumentException(domainName);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not produce message SendEmail to KAFKA or internal documentDataStorage.");
            _repository.DeleteResult(result);
            await _repository.SaveChanges(cancellationToken);
            throw new CisServiceServerErrorException(ErrorCodeMapper.ProduceSendEmailError, nameof(SendEmailHandler), "SendEmail request failed due to internal server error.");
        }

        return new SendEmailResponse { NotificationId = result.Id };
    }
}