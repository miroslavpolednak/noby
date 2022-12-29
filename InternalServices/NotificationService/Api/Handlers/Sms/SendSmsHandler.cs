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

public class SendSmsHandler : IRequestHandler<SendSmsRequest, SendSmsResponse>
{
    private readonly IDateTime _dateTime;
    private readonly McsSmsProducer _mcsSmsProducer;
    private readonly NotificationRepository _repository;
    private readonly ICodebookService _codebookService;
    private readonly IAuditLogger _auditLogger;
    private readonly ILogger<SendSmsHandler> _logger;

    public SendSmsHandler(
        IDateTime dateTime,
        McsSmsProducer mcsSmsProducer,
        NotificationRepository repository,
        ICodebookService codebookService,
        IAuditLogger auditLogger,
        ILogger<SendSmsHandler> logger)
    {
        _dateTime = dateTime;
        _mcsSmsProducer = mcsSmsProducer;
        _repository = repository;
        _codebookService = codebookService;
        _auditLogger = auditLogger;
        _logger = logger;
    }
    
    public async Task<SendSmsResponse> Handle(SendSmsRequest request, CancellationToken cancellationToken)
    {
        var smsTypes = await _codebookService.SmsNotificationTypes(new SmsNotificationTypesRequest(), cancellationToken);
        var smsType = smsTypes.FirstOrDefault(s => s.Code == request.Type) ??
        throw new CisValidationException($"Invalid Type = '{request.Type}'. Allowed Types: {string.Join(',', smsTypes.Select(s => s.Code))}");

        if (smsType.IsAuditLogEnabled)
        {
            _auditLogger.Log("todo");
        }

        var result = _repository.NewSmsResult();
        result.Identity = request.Identifier?.Identity;
        result.IdentityScheme = request.Identifier?.IdentityScheme;
        result.CustomId = request.CustomId;
        result.DocumentId = request.DocumentId;
        result.Text = request.Text;
        result.CountryCode = request.Phone.CountryCode;
        result.PhoneNumber = request.Phone.NationalNumber;

        var sendSms = new SendApi.v1.sms.SendSMS
        {
            id = result.Id.ToString(),
            phone = request.Phone.Map(),
            type = smsType.McsCode,
            text = request.Text,
            processingPriority = request.ProcessingPriority
        };

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
            // todo
        }

        return new SendSmsResponse
        {
            NotificationId = result.Id
        };
    }
}