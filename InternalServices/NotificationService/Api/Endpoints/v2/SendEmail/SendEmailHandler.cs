using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Messaging.Mappers;
using CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.v2;
using DomainServices.CodebookService.Clients;
using SharedAudit;
using SharedComponents.Storage;
using SharedTypes.Enums;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendEmail;

internal sealed class SendEmailHandler
    : IRequestHandler<SendEmailRequest, NotificationIdResponse>
{
    public async Task<NotificationIdResponse> Handle(SendEmailRequest request, CancellationToken cancellationToken)
    {
        var consumerId = _appConfiguration.Consumers.First(t => t.Username == _serviceUser.User!.Name).ConsumerId;
        var attachmentKeyFilenames = new List<(string Id, string Filename)>();
        var domainName = request.From.Value.ToLowerInvariant().Split('@').Last();

        var senderType = _appConfiguration.EmailSenders.Mcs.Contains(domainName, StringComparer.OrdinalIgnoreCase) ? Mandants.Kb
            : _appConfiguration.EmailSenders.Mpss.Contains(domainName, StringComparer.OrdinalIgnoreCase) ? Mandants.Mp
            : throw new ArgumentException(domainName);

        Database.Entities.Email result = new()
        {
            Id = Guid.NewGuid(),
            State = NotificationStates.InProgress,
            Identity = request.Identifier?.Identity,
            IdentityScheme = request.Identifier?.IdentityScheme.ToString(),
            CaseId = request.CaseId,
            CustomId = request.CustomId,
            DocumentId = request.DocumentId,
            DocumentHash = request.DocumentHash?.Hash,
            HashAlgorithm = request.DocumentHash?.HashAlgorithm.ToString(),  
            CreatedTime = _dateTime.GetLocalNow().DateTime,
            CreatedUserName = _serviceUser.User!.Name!
        };
        _dbContext.Emails.Add(result);
        // ulozit do databaze
        await _dbContext.SaveChangesAsync(cancellationToken);

        // ulozit soubory na S3
        try
        {
            foreach (var attachment in request.Attachments)
            {
                var fileId = Guid.NewGuid().ToString();
                var content = Convert.FromBase64String(attachment.Binary);
                await _storageClient.SaveFile(content, fileId, cancellationToken: cancellationToken);

                attachmentKeyFilenames.Add(new(fileId, attachment.Filename));
            }
        }
        catch (Exception ex)
        {
            _logger.SaveAttachmentFailed(result.Id, ex);
            throw new CisServiceServerErrorException(ErrorCodeMapper.UploadAttachmentFailed, nameof(SendEmailHandler), "SendEmail request failed due to internal server error.");
        }


        if (senderType == Mandants.Kb)
        {
            var sendEmail = new McsSendApi.v4.email.SendEmail
            {
                id = result.Id.ToString(),
                notificationConsumer = new()
                {
                    consumerId = consumerId
                },
                sender = request.From.MapToMcs(),
                to = request.To.MapToMcs().ToList(),
                bcc = request.Bcc.MapToMcs().ToList(),
                cc = request.Cc.MapToMcs().ToList(),
                replyTo = request.ReplyTo?.MapToMcs(),
                subject = request.Subject,
                content = new()
                {
                    charset = "UTF-8",
                    format = request.Content.Format,
                    language = request.Content.Language,
                    text = request.Content.Text
                },
                attachments = attachmentKeyFilenames
                    .Select(t => new McsSendApi.v4.Attachment()
                    {
                        s3Content = new()
                        {
                            filename = t.Filename,
                            objectKey = t.Id
                        }
                    })
                    .ToList()
            };

            await _mcsEmailProducer.SendEmail(sendEmail, cancellationToken);
        }
        else
        {
            SendEmail email = new()
            {
                Data = request
            };

            await _documentDataStorage.Add(result.Id.ToString(), email, cancellationToken);

        }
        
        return new NotificationIdResponse
        {
            NotificationId = result.Id.ToString()
        };
    }

    private readonly IStorageClient<IMcsStorage> _storageClient;
    private readonly IMcsEmailProducer _mcsEmailProducer;
    private readonly IAuditLogger _auditLogger;
    private readonly TimeProvider _dateTime;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ILogger<SendEmailHandler> _logger;
    private readonly Core.Security.IServiceUserAccessor _serviceUser;
    private readonly Database.NotificationDbContext _dbContext;
    private readonly Configuration.AppConfiguration _appConfiguration;
    private readonly IMcsSmsProducer _mcsSmsProducer;

    public SendEmailHandler(
        TimeProvider dateTime,
        ICodebookServiceClient codebookService,
        ILogger<SendEmailHandler> logger,
        Core.Security.IServiceUserAccessor serviceUser,
        Database.NotificationDbContext dbContext,
        Configuration.AppConfiguration appConfiguration,
        IMcsSmsProducer mcsSmsProducer,
        IAuditLogger auditLogger,
        IMcsEmailProducer mcsEmailProducer,
        IStorageClient<IMcsStorage> storageClient)
    {
        _dateTime = dateTime;
        _codebookService = codebookService;
        _logger = logger;
        _serviceUser = serviceUser;
        _dbContext = dbContext;
        _appConfiguration = appConfiguration;
        _mcsSmsProducer = mcsSmsProducer;
        _auditLogger = auditLogger;
        _mcsEmailProducer = mcsEmailProducer;
        _storageClient = storageClient;
    }
}
