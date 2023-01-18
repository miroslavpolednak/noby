using CIS.Core;
using CIS.Core.Exceptions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.NotificationService.Api.Services.Mcs.Mappers;
using CIS.InternalServices.NotificationService.Api.Services.Mcs.Producers;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.SmsNotificationTypes;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Sms;

public class SendSmsFromTemplateHandler : IRequestHandler<SendSmsFromTemplateRequest, SendSmsFromTemplateResponse>
{
    private readonly IDateTime _dateTime;
    private readonly McsSmsProducer _mcsSmsProducer;
    private readonly NotificationRepository _repository;
    private readonly ICodebookService _codebookService;
    private readonly IAuditLogger _auditLogger;
    private readonly ILogger<SendSmsFromTemplateHandler> _logger;
    
    public SendSmsFromTemplateHandler(
        IDateTime dateTime,
        McsSmsProducer mcsSmsProducer,
        NotificationRepository repository,
        ICodebookService codebookService,
        IAuditLogger auditLogger,
        ILogger<SendSmsFromTemplateHandler> logger)
    {
        _dateTime = dateTime;
        _mcsSmsProducer = mcsSmsProducer;
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
        
        if (smsType.IsAuditLogEnabled)
        {
            _auditLogger.Log("todo");
        }
        
        // todo: placeholders
        var text = smsType.SmsText;
        
        var result = _repository.NewSmsResult();
        result.Identity = request.Identifier?.Identity;
        result.IdentityScheme = request.Identifier?.IdentityScheme;
        result.CustomId = request.CustomId;
        result.DocumentId = request.DocumentId;
        result.RequestTimestamp = _dateTime.Now;
        
        result.Text = text;
        result.CountryCode = request.Phone.CountryCode;
        result.PhoneNumber = request.Phone.NationalNumber;

        var sendSms = new SendApi.v4.sms.SendSMS
        {
            id = result.Id.ToString(),
            phone = request.Phone.Map(),
            type = smsType.McsCode,
            text = text,
            processingPriority = request.ProcessingPriority
        };

        if (smsType.IsAuditLogEnabled)
        {
            _auditLogger.Log("todo");
        }
        
        try
        {
            await _mcsSmsProducer.SendSms(sendSms, cancellationToken);
            result.HandoverToMcsTimestamp = _dateTime.Now;

            await _repository.AddResult(result, cancellationToken);
            await _repository.SaveChanges(cancellationToken);

            if (smsType.IsAuditLogEnabled)
            {
                _auditLogger.Log("todo - sms ");
            }
        }
        catch (Exception e)
        {
            // todo:
        }

        return new SendSmsFromTemplateResponse
        {
            NotificationId = result.Id
        };
    }
}