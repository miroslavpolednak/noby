using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Services;
using CIS.InternalServices.NotificationService.Contracts.v2;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using KafkaFlow;
using SharedAudit;
using SharedComponents.DocumentDataStorage;
using System.Globalization;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendSms;

internal sealed class SendSmsHandler
    : IRequestHandler<SendSmsRequest, NotificationIdResponse>
{
    public async Task<NotificationIdResponse> Handle(SendSmsRequest request, CancellationToken cancellationToken)
    {
        var notificationId = Guid.NewGuid();
        _logger.NotificationRequestReceived(notificationId, NotificationChannels.Sms);

        var smsType = await getAndValidateSmsType(request.Type, cancellationToken);
        _ = request.PhoneNumber.TryParsePhone(out string? countryCode, out string? nationalNumber);

        // pripravit zpravu do databaze
        Database.Entities.Notification notificationInstance = new()
        {
            Id = notificationId,
            State = NotificationStates.InProgress,
            Channel = NotificationChannels.Sms,
            Identity = request.Identifier?.Identity,
            IdentityScheme = request.Identifier?.IdentityScheme.ToString(),
            CaseId = request.CaseId,
            CustomId = request.CustomId,
            DocumentId = request.DocumentId,
            DocumentHash = request.DocumentHash?.Hash,
            HashAlgorithm = request.DocumentHash?.HashAlgorithm.ToString(),
            CreatedTime = _dateTime.GetLocalNow().DateTime,
            CreatedUserName = _serviceUser.UserName
        };
        _dbContext.Add(notificationInstance);
        // ulozit do databaze
        await _dbContext.SaveChangesAsync(cancellationToken);

        // ulozit obsah SMS
        await _documentDataStorage.Add(notificationInstance.Id.ToString(), new Database.DocumentDataEntities.SmsData
        {
            CountryCode = countryCode!,
            NationalNumber = nationalNumber!,
            Text = request.Text,
            SmsType = smsType.Code,
            ProcessingPriority = request.ProcessingPriority
        }, cancellationToken);

        // pripravit zpravu do MCS
        var message = new McsSendApi.v4.sms.SendSMS
        {
            id = notificationInstance.Id.ToString(),
            phone = new()
            {
                countryCode = countryCode,
                nationalPhoneNumber = nationalNumber
            },
            type = smsType.McsCode,
            text = request.Text,
            processingPriority = request.ProcessingPriority,
            notificationConsumer = new()
            {
                 consumerId = _serviceUser.ConsumerId
            }
        };

        try
        {
            // odeslat do MCS
            await _mcsSmsProducer.ProduceAsync(message.id, message);

            // nastavit stav v databazi
            notificationInstance.State = NotificationStates.Sent;
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.NotificationSent(notificationInstance.Id, NotificationChannels.Sms);
            createAuditLog(request, smsType, _serviceUser.ConsumerId, notificationInstance.Id, true);
        }
        catch (Exception ex)
        {
            // nastavit stav v databazi
            notificationInstance.State = NotificationStates.Error;
            notificationInstance.Errors = [ new() { Code = "1", Message = "Unable to send message to Kafka" } ];
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.NotificationFailedToSend(notificationInstance.Id, NotificationChannels.Sms, ex);
            createAuditLog(request, smsType, _serviceUser.ConsumerId, notificationInstance.Id, false, ex.Message);

            throw new CIS.Core.Exceptions.ExternalServices.CisExternalServiceUnavailableException(0, "MCS");
        }

        return new NotificationIdResponse 
        { 
            NotificationId = message.id
        };
    }

    private void createAuditLog(
        SendSmsRequest request, 
        in SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, 
        in string consumerId, 
        in Guid notificationId,
        in bool isSuccesful,
        in string? errorMessage = null)
    {
        if (smsType.IsAuditLogEnabled)
        {
            var bodyBefore = new Dictionary<string, string>
            {
                { "smsType", smsType.Code },
                { "consumer", consumerId },
                { "serviceUserName", _serviceUser.UserName },
                { "phoneNumber", request.PhoneNumber },
                { "processingPriority", request.ProcessingPriority?.ToString(CultureInfo.InvariantCulture) ?? "" },
                { "type", request.Type },
                { "text", request.Text },
                { "identityId", request.Identifier?.Identity ?? string.Empty },
                { "identityScheme", request.Identifier?.IdentityScheme.ToString() ?? string.Empty },
                { "caseId", request.CaseId?.ToString(CultureInfo.InvariantCulture) ?? string.Empty },
                { "customId", request.CustomId ?? string.Empty },
                { "documentId", request.DocumentId ?? string.Empty },
                { "documentHash", request.DocumentHash?.Hash ?? string.Empty },
                { "documentHashAlgorithm", request.DocumentHash?.HashAlgorithm.ToString() ?? string.Empty }
            };

            if (!isSuccesful)
            {
                bodyBefore.Add("errorMessage", errorMessage ?? "");
            }

            _auditLogger.Log(
                AuditEventTypes.Noby013,
                isSuccesful ? "Produced message SendSMS to KAFKA" : "Could not produce message SendSMS to KAFKA",
                bodyBefore: bodyBefore,
                bodyAfter: new Dictionary<string, string>
                {
                    { "notificationId", notificationId.ToString() }
                }
            );
        }
    }

    /// <summary>
    /// Ziskat typ notifikace z ciselniku
    /// </summary>
    private async Task<SmsNotificationTypesResponse.Types.SmsNotificationTypeItem> getAndValidateSmsType(string smsType, CancellationToken cancellationToken)
    {
        var smsTypes = await _codebookService.SmsNotificationTypes(cancellationToken);
        return smsTypes
            .FirstOrDefault(s => s.Code == smsType) ??
            throw new CisValidationException($"Invalid Type = '{smsType}'. Allowed Types: {string.Join("; ", smsTypes.Select(s => s.Code))}");
    }

    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly IAuditLogger _auditLogger;
    private readonly TimeProvider _dateTime;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ILogger<SendSmsHandler> _logger;
    private readonly ServiceUserHelper _serviceUser;
    private readonly Database.NotificationDbContext _dbContext;
    private readonly IMessageProducer<cz.kb.osbs.mcs.sender.sendapi.v4.sms.SendSMS> _mcsSmsProducer;

    public SendSmsHandler(
        TimeProvider dateTime,
        ICodebookServiceClient codebookService,
        ILogger<SendSmsHandler> logger,
        ServiceUserHelper serviceUser,
        Database.NotificationDbContext dbContext,
        IMessageProducer<cz.kb.osbs.mcs.sender.sendapi.v4.sms.SendSMS> mcsSmsProducer,
        IAuditLogger auditLogger,
        IDocumentDataStorage documentDataStorage)
    {
        _dateTime = dateTime;
        _codebookService = codebookService;
        _logger = logger;
        _serviceUser = serviceUser;
        _dbContext = dbContext;
        _mcsSmsProducer = mcsSmsProducer;
        _auditLogger = auditLogger;
        _documentDataStorage = documentDataStorage;
    }
}
