using CIS.Core;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.v2;
using CIS.InternalServices.NotificationService.LegacyContracts.Result.Dto;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;
using SharedAudit;
using System.Globalization;
using CIS.InternalServices.NotificationService.Api.Helpers;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v2.SendSms;

internal sealed class SendSmsHandler
    : IRequestHandler<SendSmsRequest, NotificationIdResponse>
{
    public async Task<NotificationIdResponse> Handle(SendSmsRequest request, CancellationToken cancellationToken)
    {
        var smsType = await getSmsType(request.Sms.Type, cancellationToken);
        string userName = _serviceUser.User!.Name!;
        var phone = request.Sms.PhoneNumber.ParsePhone()!;

        Database.Entities.SmsResult result = new()
        {
            Id = Guid.NewGuid(),
            Channel = NotificationChannel.Sms,
            State = NotificationState.InProgress,
            Identity = request.Sms.Identifier?.Identity,
            IdentityScheme = request.Sms.Identifier?.IdentityScheme.ToString(),
            CaseId = request.Sms.CaseId,
            CustomId = request.Sms.CustomId,
            DocumentId = request.Sms.DocumentId,
            DocumentHash = request.Sms.DocumentHash?.Hash,
            HashAlgorithm = request.Sms.DocumentHash?.HashAlgorithm.ToString(),
            RequestTimestamp = _dateTime.Now,
            Type = request.Sms.Type,
            CountryCode = phone.CountryCode,
            PhoneNumber = phone.NationalNumber,
            CreatedBy = userName
        };

        _dbContext.SmsResults.Add(result);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var consumerId = _appConfiguration.Consumers.First(t => t.Username == _serviceUser.User.Name).ConsumerId;

        var sendSms = new McsSendApi.v4.sms.SendSMS
        {
            id = result.Id.ToString(),
            phone = new()
            {
                countryCode = phone.CountryCode,
                nationalPhoneNumber = phone.NationalNumber
            },
            type = smsType.McsCode,
            text = request.Text,
            processingPriority = request.Sms.ProcessingPriority,
            notificationConsumer = new()
            {
                 consumerId = consumerId
            }
        };

        try
        {
            await _mcsSmsProducer.SendSms(sendSms, cancellationToken);

            createAuditLog(request, smsType, consumerId, result.Id);
        }
        catch
        {
            createAuditLog(request, smsType, consumerId, result.Id);

            _dbContext.SmsResults.Remove(result);
            await _dbContext.SaveChangesAsync(cancellationToken);

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
                    { "identity", request.Sms.Identifier?.Identity ?? string.Empty },
                    { "identityScheme", request.Sms.Identifier?.IdentityScheme.ToString() ?? string.Empty },
                    { "caseId", request.Sms.CaseId?.ToString(CultureInfo.InvariantCulture) ?? string.Empty },
                    { "customId", request.Sms.CustomId ?? string.Empty },
                    { "documentId", request.Sms.DocumentId ?? string.Empty },
                    { "documentHash", request.Sms.DocumentHash?.Hash ?? string.Empty },
                    { "hashAlgorithm", request.Sms.DocumentHash?.HashAlgorithm.ToString() ?? string.Empty }
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
    private readonly IDateTime _dateTime;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ILogger<SendSmsHandler> _logger;
    private readonly Core.Security.IServiceUserAccessor _serviceUser;
    private readonly Database.NotificationDbContext _dbContext;
    private readonly Configuration.AppConfiguration _appConfiguration;
    private readonly IMcsSmsProducer _mcsSmsProducer;

    public SendSmsHandler(
        IDateTime dateTime,
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
