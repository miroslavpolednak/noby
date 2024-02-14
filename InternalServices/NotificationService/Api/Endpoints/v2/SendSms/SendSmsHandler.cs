using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.v2;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using SharedAudit;
using System.Globalization;
using CIS.InternalServices.NotificationService.Api.Extensions;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendSms;

internal sealed class SendSmsHandler
    : IRequestHandler<SendSmsRequest, NotificationIdResponse>
{
    public async Task<NotificationIdResponse> Handle(SendSmsRequest request, CancellationToken cancellationToken)
    {
        var consumerId = _appConfiguration.Consumers.First(t => t.Username == _serviceUser.User!.Name).ConsumerId;
        var smsType = await getSmsType(request.Type, cancellationToken);
        var (countryCode, nationalNumber) = request.PhoneNumber.ParsePhone();

        // pripravit zpravu do databaze
        Database.Entities.Sms result = new()
        {
            Id = Guid.NewGuid(),
            State = NotificationStates.InProgress,
            Text = request.Text,
            Identity = request.Identifier?.Identity,
            IdentityScheme = request.Identifier?.IdentityScheme.ToString(),
            CaseId = request.CaseId,
            CustomId = request.CustomId,
            DocumentId = request.DocumentId,
            DocumentHash = request.DocumentHash?.Hash,
            HashAlgorithm = request.DocumentHash?.HashAlgorithm.ToString(),
            Type = request.Type,
            CountryCode = countryCode,
            PhoneNumber = nationalNumber,
            CreatedTime = _dateTime.GetLocalNow().DateTime,
            CreatedUserName = _serviceUser.User!.Name!
        };
        _dbContext.Sms.Add(result);
        // ulozit do databaze
        await _dbContext.SaveChangesAsync(cancellationToken);

        // pripravit zpravu do MCS
        var sendSms = new McsSendApi.v4.sms.SendSMS
        {
            id = result.Id.ToString(),
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
                 consumerId = consumerId
            }
        };

        try
        {
            // odeslat do MCS
            await _mcsSmsProducer.SendSms(sendSms, cancellationToken);

            // nastavit stav v databazi
            result.State = NotificationStates.Sent;
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.NotificationSent(result.Id, NotificationChannels.Sms);
            createAuditLog(request, smsType, consumerId, result.Id);
        }
        catch (Exception ex)
        {
            // nastavit stav v databazi
            result.State = NotificationStates.Error;
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.NotificationFailedToSend(result.Id, NotificationChannels.Sms, ex);
            createAuditLog(request, smsType, consumerId);
            throw;
        }

        return new NotificationIdResponse 
        { 
            NotificationId = result.Id.ToString()
        };
    }

    private void createAuditLog(SendSmsRequest request, SmsNotificationTypesResponse.Types.SmsNotificationTypeItem smsType, in string consumerId, in Guid? result = null)
    {
        if (smsType.IsAuditLogEnabled)
        {
            _auditLogger.Log(
                AuditEventTypes.Noby013,
                result.HasValue ? "Produced message SendSMS to KAFKA" : "Could not produce message SendSMS to KAFKA",
                bodyBefore: new Dictionary<string, string>
                {
                    { "smsType", smsType.Code },
                    { "consumer", consumerId },
                    { "identity", request.Identifier?.Identity ?? string.Empty },
                    { "identityScheme", request.Identifier?.IdentityScheme.ToString() ?? string.Empty },
                    { "caseId", request.CaseId?.ToString(CultureInfo.InvariantCulture) ?? string.Empty },
                    { "customId", request.CustomId ?? string.Empty },
                    { "documentId", request.DocumentId ?? string.Empty },
                    { "documentHash", request.DocumentHash?.Hash ?? string.Empty },
                    { "hashAlgorithm", request.DocumentHash?.HashAlgorithm.ToString() ?? string.Empty }
                },
                bodyAfter: result.HasValue ? new Dictionary<string, string>
                {
                    { "notificationId", result.Value.ToString() }
                } : null
            );
        }
    }

    private async Task<SmsNotificationTypesResponse.Types.SmsNotificationTypeItem> getSmsType(string smsType, CancellationToken cancellationToken)
    {
        var smsTypes = await _codebookService.SmsNotificationTypes(cancellationToken);
        return smsTypes
            .FirstOrDefault(s => s.Code == smsType) ??
            throw new CisValidationException($"Invalid Type = '{smsType}'. Allowed Types: {string.Join("; ", smsTypes.Select(s => s.Code))}");
    }

    private readonly IAuditLogger _auditLogger;
    private readonly TimeProvider _dateTime;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ILogger<SendSmsHandler> _logger;
    private readonly Core.Security.IServiceUserAccessor _serviceUser;
    private readonly Database.NotificationDbContext _dbContext;
    private readonly Configuration.AppConfiguration _appConfiguration;
    private readonly IMcsSmsProducer _mcsSmsProducer;

    public SendSmsHandler(
        TimeProvider dateTime,
        ICodebookServiceClient codebookService,
        ILogger<SendSmsHandler> logger,
        Core.Security.IServiceUserAccessor serviceUser,
        Database.NotificationDbContext dbContext,
        Configuration.AppConfiguration appConfiguration,
        IMcsSmsProducer mcsSmsProducer,
        IAuditLogger auditLogger)
    {
        _dateTime = dateTime;
        _codebookService = codebookService;
        _logger = logger;
        _serviceUser = serviceUser;
        _dbContext = dbContext;
        _appConfiguration = appConfiguration;
        _mcsSmsProducer = mcsSmsProducer;
        _auditLogger = auditLogger;
    }
}
