using CIS.Core.Exceptions.ExternalServices;
using CIS.InternalServices.NotificationService.Api.Services;
using CIS.InternalServices.NotificationService.Contracts.v2;
using KafkaFlow;
using SharedAudit;
using SharedComponents.DocumentDataStorage;
using SharedComponents.Storage;
using System.Globalization;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendEmail;

internal sealed class SendEmailHandler(
    TimeProvider _dateTime,
    ILogger<SendEmailHandler> _logger,
    ServiceUserHelper _serviceUser,
    Database.NotificationDbContext _dbContext,
    Configuration.AppConfiguration _appConfiguration,
    IAuditLogger _auditLogger,
    IMessageProducer<cz.kb.osbs.mcs.sender.sendapi.v4.email.SendEmail> _mcsEmailProducer,
    IStorageClient<IMcsStorage> _storageClient,
    IDocumentDataStorage _documentDataStorage)
        : IRequestHandler<SendEmailRequest, NotificationIdResponse>
{
    public async Task<NotificationIdResponse> Handle(SendEmailRequest request, CancellationToken cancellationToken)
    {
        var notificationId = Guid.NewGuid();
        _logger.NotificationRequestReceived(notificationId, NotificationChannels.Email);

        var domainName = request.From.Value.GetDomainFromEmail();
        // kontrola na domenu uz je ve validatoru
        var senderType = _appConfiguration.EmailSenders.Mcs.Contains(domainName, StringComparer.OrdinalIgnoreCase) ? Mandants.Kb : Mandants.Mp;

        Database.Entities.Notification notificationInstance = new()
        {
            Id = notificationId,
            Channel = NotificationChannels.Email,
            State = NotificationStates.InProgress,
            Identity = request.Identifier?.Identity,
            IdentityScheme = request.Identifier?.IdentityScheme,
            CaseId = request.CaseId,
            CustomId = request.CustomId,
            DocumentId = request.DocumentId,
            DocumentHash = request.DocumentHash?.Hash,
            HashAlgorithm = request.DocumentHash?.HashAlgorithm,  
            CreatedTime = _dateTime.GetLocalNow().DateTime,
            CreatedUserName = _serviceUser.UserName,
            Mandant = senderType
        };
        _dbContext.Add(notificationInstance);
        // ulozit do databaze
        await _dbContext.SaveChangesAsync(cancellationToken);

        // ulozit soubory na S3 (jen pro MCS)
        var attachmentKeyFilenames = new List<(string Id, string Filename)>();
        
        // ulozit obsah emailu
        await _documentDataStorage.Add(notificationInstance.Id.ToString(), new Database.DocumentDataEntities.EmailData
        {
            Subject = request.Subject,
            Text = request.Content.Text,
            Sender = request.From,
            To = request.To.ToList(),
            Cc = request.Cc?.ToList(),
            Bcc = request.Bcc?.ToList(),
            ReplyTo = request.ReplyTo,
            Format = request.Content.Format,
            Language = request.Content.Language,
            IsAuditable = request.IsAuditable,
            Attachments = request.Attachments?.Select(t => new Database.DocumentDataEntities.EmailData.EmailAttachment
            {
                Filename = t.Filename,
                Data = t.Binary.ToBase64()
            })
            .ToList()
        }, cancellationToken);

        if (senderType == Mandants.Kb)
        {
            try
            {
                if (request.Attachments != null)
                {
                    foreach (var attachment in request.Attachments)
                    {
                        var fileId = Guid.NewGuid().ToString();
                        var content = attachment.Binary.ToArray();
                        await _storageClient.SaveFile(content, fileId, cancellationToken: cancellationToken);

                        attachmentKeyFilenames.Add(new(fileId, attachment.Filename));
                    }
                }                
            }
            catch (Exception ex)
            {
                _logger.SaveAttachmentFailed(notificationInstance.Id, ex);

                throw new CisExternalServiceServerErrorException(ErrorCodeMapper.UploadAttachmentFailed, nameof(SendEmailHandler), "Unable to upload attachment to storage service");
            }

            var message = new McsSendApi.v4.email.SendEmail
            {
                id = notificationInstance.Id.ToString(),
                notificationConsumer = new()
                {
                    consumerId = _serviceUser.ConsumerId
                },
                sender = request.From.MapToMcs(),
                to = request.To?.Select(t => t.MapToMcs()).ToList(),
                bcc = request.Bcc?.Select(t => t.MapToMcs()).ToList(),
                cc = request.Cc?.Select(t => t.MapToMcs()).ToList(),
                replyTo = request.ReplyTo?.MapToMcs(),
                subject = request.Subject,
                content = new()
                {
                    charset = "UTF-8",
                    format = getFormat(request.Content.Format),
                    language = getLanguage(request.Content.Language),
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

            try
            {
                await _mcsEmailProducer.ProduceAsync(message.id, message);

                // nastavit stav v databazi
                notificationInstance.State = NotificationStates.Sent;
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.NotificationSent(notificationInstance.Id, NotificationChannels.Email);
                if (request.IsAuditable)
                {
                    createAuditLog(request, _serviceUser.ConsumerId, notificationInstance.Id, true);
                }
            }
            catch (Exception ex)
            {
                // nastavit stav v databazi
                notificationInstance.State = NotificationStates.Error;
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.NotificationFailedToSend(notificationInstance.Id, NotificationChannels.Email, ex);
                if (request.IsAuditable)
                {
                    createAuditLog(request, _serviceUser.ConsumerId, notificationInstance.Id, false, ex.Message);
                }
                
                throw new CIS.Core.Exceptions.ExternalServices.CisExternalServiceUnavailableException(0, "MCS"); ;
            }
        } else if (request.IsAuditable) // mpss vetev - jen logujeme, email odesle job
        {
            createAuditLog(request, _serviceUser.ConsumerId, notificationInstance.Id, true);
        }
        
        return new NotificationIdResponse
        {
            NotificationId = notificationInstance.Id.ToString()
        };
    }

    private void createAuditLog(
        SendEmailRequest request,
        in string consumerId,
        in Guid notificationId,
        in bool isSuccesful,
        in string? errorMessage = null)
    {
        var bodyBefore = new Dictionary<string, string>
        {
            { "subject", request.Subject },
            { "text", request.Content.Text },
            { "consumer", consumerId },
            { "serviceUserName", _serviceUser.UserName },
            { "identityId", request.Identifier?.Identity ?? string.Empty },
            { "identityScheme", request.Identifier?.IdentityScheme.ToString() ?? string.Empty },
            { "caseId", request.CaseId?.ToString(CultureInfo.InvariantCulture) ?? string.Empty },
            { "customId", request.CustomId ?? string.Empty },
            { "documentId", request.DocumentId ?? string.Empty },
            { "documentHash", request.DocumentHash?.Hash ?? string.Empty },
            { "documentHashAlgorithm", request.DocumentHash?.HashAlgorithm.ToString() ?? string.Empty }
        };

        addToBody("from", request.From);
        addToBody("to", request.To);
        addToBody("cc", request.Cc);
        addToBody("bcc", request.Bcc);
        addToBody("replyTo", request.ReplyTo);

        if (!isSuccesful)
        {
            bodyBefore.Add("errorMessage", errorMessage ?? "");
        }

        _auditLogger.Log(
            AuditEventTypes.Noby013,
            isSuccesful ? "Produced message SendEmail to KAFKA" : "Could not produce message SendEmail to KAFKA",
            bodyBefore: bodyBefore,
            bodyAfter: new Dictionary<string, string>
            {
                { "notificationId", notificationId.ToString() }
            }
        );

        void addToBody<TData>(in string key, TData? data)
        {
            if (data is not null)
            {
                bodyBefore.Add(key, System.Text.Json.JsonSerializer.Serialize(data));
            }
        }
    }

    private static string getLanguage(Contracts.v2.SendEmailRequest.Types.EmailLanguages language)
        => language == SendEmailRequest.Types.EmailLanguages.En ? "en" : "cs";

    private static string getFormat(Contracts.v2.SendEmailRequest.Types.EmailFormats format)
        => format == SendEmailRequest.Types.EmailFormats.PlainText ? "text/plain" : "text/html";
}
