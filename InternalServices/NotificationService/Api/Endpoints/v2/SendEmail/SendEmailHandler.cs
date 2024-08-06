using CIS.Core.Exceptions.ExternalServices;
using CIS.InternalServices.NotificationService.Api.Database;
using CIS.InternalServices.NotificationService.Api.Services;
using CIS.InternalServices.NotificationService.Contracts.v2;
using KafkaFlow;
using SharedAudit;
using SharedComponents.DocumentDataStorage;
using SharedComponents.Storage;
using System.Text.Json;

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

        // zmenit emailovou adresu odesilatele, pokud je namapovana
        request.From.Value = request.From.Value.ResolveSenderEmail(_appConfiguration.EmailSenders.AddressMapping);

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
            ProductId = request.Product?.ProductId,
            ProductType = request.Product?.ProductType,
            CustomId = request.CustomId,
            DocumentId = request.DocumentId,
            DocumentHashes = request.DocumentHashes,
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
                id = Configuration.KafkaTopics.McsIdPrefix + notificationInstance.Id.ToString(),
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

                createAuditLog(request, _serviceUser.ConsumerId, notificationInstance.Id, true);
            }
            catch (Exception ex)
            {
                // nastavit stav v databazi
                notificationInstance.State = NotificationStates.Error;
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.NotificationFailedToSend(notificationInstance.Id, NotificationChannels.Email, ex);

                createAuditLog(request, _serviceUser.ConsumerId, notificationInstance.Id, false, ex.Message);

                throw new CIS.Core.Exceptions.ExternalServices.CisExternalServiceUnavailableException(0, "MCS"); ;
            }
        } else // mpss vetev - jen logujeme, email odesle job
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
            { "from", request.From.Value },
            { "to", addValue(request.To.Select(t => t.Value)) },
            { "consumer", consumerId },
            { "serviceUserName", _serviceUser.UserName },
            { "identityId", request.Identifier?.Identity ?? string.Empty },
            { "identityScheme", request.Identifier?.IdentityScheme.ToString() ?? string.Empty },
            { "customId", request.CustomId ?? string.Empty },
            { "documentId", request.DocumentId ?? string.Empty },
            { "documentHashes", addValue(request.DocumentHashes) },
        };

        if (!isSuccesful)
        {
            bodyBefore.Add("errorMessage", errorMessage ?? "");
        }

        _auditLogger.Log(
            AuditEventTypes.Noby013,
            isSuccesful ? "Produced message SendEmail to KAFKA" : "Could not produce message SendEmail to KAFKA",
            products: request.Product?.ToAuditLoggerHeaderItems(),
            bodyBefore: bodyBefore,
            bodyAfter: new Dictionary<string, string>
            {
                { "notificationId", notificationId.ToString() }
            }
        );

        string addValue<TData>(TData? data)
            => JsonSerializer.Serialize(data, EntitiesExtensions._jsonSerializerOptions) ?? string.Empty;
    }

    private static string getLanguage(Contracts.v2.SendEmailRequest.Types.EmailLanguages language)
        => language == SendEmailRequest.Types.EmailLanguages.En ? "en" : "cs";

    private static string getFormat(Contracts.v2.SendEmailRequest.Types.EmailFormats format)
        => format == SendEmailRequest.Types.EmailFormats.PlainText ? "text/plain" : "text/html";
}
