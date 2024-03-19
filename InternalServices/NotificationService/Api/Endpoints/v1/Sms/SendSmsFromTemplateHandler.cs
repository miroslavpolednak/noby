using CIS.Core;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Helpers;
using CIS.InternalServices.NotificationService.Api.Messaging.Mappers;
using CIS.InternalServices.NotificationService.Api.Messaging.Producers.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.AuditLog.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using DomainServices.CodebookService.Clients;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.v1.Sms;

public class SendSmsFromTemplateHandler : IRequestHandler<SendSmsFromTemplateRequest, SendSmsFromTemplateResponse>
{
    private const int _maxSmsTextLength = 480;
    private readonly TimeProvider _dateTime;
    private readonly IMcsSmsProducer _mcsSmsProducer;
    private readonly IUserAdapterService _userAdapterService;
    private readonly INotificationRepository _repository;
    private readonly ICodebookServiceClient _codebookService;
    private readonly ISmsAuditLogger _smsAuditLogger;
    private readonly ILogger<SendSmsFromTemplateHandler> _logger;
    
    public SendSmsFromTemplateHandler(
        TimeProvider dateTime,
        IMcsSmsProducer mcsSmsProducer,
        IUserAdapterService userAdapterService,
        INotificationRepository repository,
        ICodebookServiceClient codebookService,
        ISmsAuditLogger smsAuditLogger,
        ILogger<SendSmsFromTemplateHandler> logger)
    {
        _dateTime = dateTime;
        _mcsSmsProducer = mcsSmsProducer;
        _userAdapterService = userAdapterService;
        _repository = repository;
        _codebookService = codebookService;
        _smsAuditLogger = smsAuditLogger;
        _logger = logger;
    }
    
    public async Task<SendSmsFromTemplateResponse> Handle(SendSmsFromTemplateRequest request, CancellationToken cancellationToken)
    {
        var username = _userAdapterService
            .CheckSendSmsAccess()
            .GetUsername();

        var smsTypes = await _codebookService.SmsNotificationTypes(cancellationToken);
        var smsTypeCodes = string.Join(", ", smsTypes.Select(s => s.Code));
        var smsType = smsTypes.FirstOrDefault(s => s.Code == request.Type) ??
                      throw new CisValidationException($"Invalid Type = '{request.Type}'. Allowed Types: {smsTypeCodes}");

        var hashAlgorithms = await _codebookService.HashAlgorithms(cancellationToken);
        var hashAlgorithmCodes = string.Join(", ", hashAlgorithms.Select(s => s.Code));
        var hashAlgorithm = string.IsNullOrEmpty(request.DocumentHash?.HashAlgorithm)
            ? null
            : hashAlgorithms.FirstOrDefault(s => s.Code == request.DocumentHash.HashAlgorithm) ?? 
              throw new CisValidationException($"Invalid HashAlgorithm = '{request.DocumentHash.HashAlgorithm}'. Allowed HashAlgorithms: {hashAlgorithmCodes}");

        if (string.IsNullOrEmpty(smsType.SmsText))
        {
            throw new CisValidationException($"Invalid Type = '{request.Type}' has empty template text.");
        }
        
        var keyValues = request.Placeholders.ToDictionary(p => p.Key, p => p.Value);
        
        smsType.SmsText.Validate(keyValues.Keys);
        var text = smsType.SmsText.Interpolate(keyValues).ToGSMString();

        if (text.Length > _maxSmsTextLength)
        {
            throw new CisValidationException($"Final sms text from template '{text}' is too long. Maximum allowed length is {_maxSmsTextLength}.");
        }
        
        var result = _repository.NewSmsResult();
        var phone = request.PhoneNumber.ParsePhone()!;
        result.Identity = request.Identifier?.Identity;
        result.IdentityScheme = request.Identifier?.IdentityScheme;
        result.CaseId = request.CaseId;
        result.CustomId = request.CustomId;
        result.DocumentId = request.DocumentId;
        result.DocumentHash = request.DocumentHash?.Hash;
        result.HashAlgorithm = request.DocumentHash?.HashAlgorithm;
        result.RequestTimestamp = _dateTime.GetLocalNow().DateTime;

        result.Type = request.Type;
        result.CountryCode = phone.CountryCode;
        result.PhoneNumber = phone.NationalNumber;

        result.CreatedBy = username;
        
        try
        {
            await _repository.AddResult(result, cancellationToken);
            await _repository.SaveChanges(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not create SmsResult.");
            throw new CisServiceServerErrorException(ErrorHandling.ErrorCodeMapper.CreateSmsResultFailed, nameof(SendSmsFromTemplateHandler), "SendSmsFromTemplate request failed due to internal server error.");
        }
        
        var consumerId = _userAdapterService.GetConsumerId();
        
        var sendSms = new McsSendApi.v4.sms.SendSMS
        {
            id = result.Id.ToString(),
            phone = phone.Map(),
            type = smsType.McsCode,
            text = text,
            processingPriority = request.ProcessingPriority,
            notificationConsumer = McsSmsMappers.MapToMcs(consumerId)
        };
        
        try
        {
            await _mcsSmsProducer.SendSms(sendSms, cancellationToken);
            _smsAuditLogger.LogKafkaProduced(smsType, result.Id, username,
                request.Identifier?.Identity,
                request.Identifier?.IdentityScheme,
                request.CaseId,
                request.CustomId,
                request.DocumentId,
                request.DocumentHash?.Hash,
                request.DocumentHash?.HashAlgorithm);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not produce message SendSMS to KAFKA.");
            _smsAuditLogger.LogKafkaProduceError(smsType, username,
                request.Identifier?.Identity,
                request.Identifier?.IdentityScheme,
                request.CaseId,
                request.CustomId,
                request.DocumentId,
                request.DocumentHash?.Hash,
                request.DocumentHash?.HashAlgorithm);
            _repository.DeleteResult(result);
            await _repository.SaveChanges(cancellationToken);
            throw new CisServiceServerErrorException(ErrorHandling.ErrorCodeMapper.ProduceSendSmsError, nameof(SendSmsFromTemplateHandler), "SendSmsFromTemplate request failed due to internal server error.");
        }

        return new SendSmsFromTemplateResponse { NotificationId = result.Id };
    }
}