using CIS.Core;
using CIS.Core.Exceptions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.NotificationService.Api.Helpers;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Mappers;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers;
using CIS.InternalServices.NotificationService.Api.Services.Messaging.Producers.Infrastructure;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.SmsNotificationTypes;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Sms;

public class SendSmsFromTemplateHandler : IRequestHandler<SendSmsFromTemplateRequest, SendSmsFromTemplateResponse>
{
    private const int _maxSmsTextLength = 480;
    private readonly IDateTime _dateTime;
    private readonly McsSmsProducer _mcsSmsProducer;
    private readonly UserConsumerIdMapper _userConsumerIdMapper;
    private readonly NotificationRepository _repository;
    private readonly ICodebookService _codebookService;
    private readonly IAuditLogger _auditLogger;
    private readonly ILogger<SendSmsFromTemplateHandler> _logger;
    
    public SendSmsFromTemplateHandler(
        IDateTime dateTime,
        McsSmsProducer mcsSmsProducer,
        UserConsumerIdMapper userConsumerIdMapper,
        NotificationRepository repository,
        ICodebookService codebookService,
        IAuditLogger auditLogger,
        ILogger<SendSmsFromTemplateHandler> logger)
    {
        _dateTime = dateTime;
        _mcsSmsProducer = mcsSmsProducer;
        _userConsumerIdMapper = userConsumerIdMapper;
        _repository = repository;
        _codebookService = codebookService;
        _auditLogger = auditLogger;
        _logger = logger;
    }
    
    public async Task<SendSmsFromTemplateResponse> Handle(SendSmsFromTemplateRequest request, CancellationToken cancellationToken)
    {
        var smsTypes = await _codebookService.SmsNotificationTypes(new SmsNotificationTypesRequest(), cancellationToken);
        var smsType = smsTypes.FirstOrDefault(s => s.Code == request.Type) ??
        throw new CisValidationException($"Invalid Type = '{request.Type}'. Allowed Types: {string.Join(',', smsTypes.Select(s => s.Code))}");

        if (string.IsNullOrEmpty(smsType.SmsText))
        {
            throw new CisValidationException($"Invalid Type = '{request.Type}' has empty template text.");
        }
        
        var auditEnabled = smsType.IsAuditLogEnabled;
        var keyValues = request.Placeholders.ToDictionary(p => p.Key, p => p.Value);
        
        smsType.SmsText.Validate(keyValues.Keys);
        var text = smsType.SmsText.Interpolate(keyValues);

        if (text.Length > _maxSmsTextLength)
        {
            throw new CisValidationException($"Final sms text from template '{text}' is too long. Maximum allowed length is {_maxSmsTextLength}.");
        }
        
        var result = _repository.NewSmsResult();
        result.Identity = request.Identifier?.Identity;
        result.IdentityScheme = request.Identifier?.IdentityScheme;
        result.CustomId = request.CustomId;
        result.DocumentId = request.DocumentId;
        result.RequestTimestamp = _dateTime.Now;

        result.Type = request.Type;
        result.Text = text;
        result.CountryCode = request.Phone.CountryCode;
        result.PhoneNumber = request.Phone.NationalNumber;

        result.CreatedBy = "todo";
        
        try
        {
            await _repository.AddResult(result, cancellationToken);
            await _repository.SaveChanges(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not create SmsResult.");
            throw new CisServiceServerErrorException(ErrorCodes.Internal.CreateSmsResultFailed, nameof(SendSmsFromTemplateHandler), "SendSmsFromTemplate request failed due to internal server error.");
        }
        
        var consumerId = _userConsumerIdMapper.GetConsumerId();
        
        var sendSms = new McsSendApi.v4.sms.SendSMS
        {
            id = result.Id.ToString(),
            phone = request.Phone.Map(),
            type = smsType.McsCode,
            text = text,
            processingPriority = request.ProcessingPriority,
            notificationConsumer = McsSmsMappers.MapToMcs(consumerId)
        };
        
        try
        {
            if (auditEnabled)
            {
                _auditLogger.Log("todo - Producing message SendSMS to KAFKA.");
            }
            
            await _mcsSmsProducer.SendSms(sendSms, cancellationToken);

            if (auditEnabled)
            {
                _auditLogger.Log("todo - Produced message SendSMS to KAFKA.");
            }
        }
        catch (Exception e)
        {
            if (auditEnabled)
            {
                _auditLogger.Log("todo - Could not produce message SendSMS to KAFKA.");
            }
            
            _logger.LogError(e, "Could not produce message SendSMS to KAFKA.");
            _repository.DeleteResult(result);
            await _repository.SaveChanges(cancellationToken);
            throw new CisServiceServerErrorException(ErrorCodes.Internal.ProduceSendSmsError, nameof(SendSmsFromTemplateHandler), "SendSmsFromTemplate request failed due to internal server error.");
        }

        return new SendSmsFromTemplateResponse { NotificationId = result.Id };
    }
}