using System.Text.Json;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.NotificationService.Api.Services.Mcs.Mappers;
using CIS.InternalServices.NotificationService.Api.Services.Mcs.Producers;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Sms;

public class SendSmsFromTemplateHandler : IRequestHandler<SendSmsFromTemplateRequest, SendSmsFromTemplateResponse>
{
    private readonly McsSmsProducer _mcsSmsProducer;
    private readonly NotificationRepository _repository;
    private readonly IAuditLogger _auditLogger;
    private readonly ILogger<SendSmsFromTemplateHandler> _logger;
    
    public SendSmsFromTemplateHandler(
        McsSmsProducer mcsSmsProducer,
        NotificationRepository repository,
        IAuditLogger auditLogger,
        ILogger<SendSmsFromTemplateHandler> logger)
    {
        _mcsSmsProducer = mcsSmsProducer;
        _repository = repository;
        _auditLogger = auditLogger;
        _logger = logger;
    }
    
    public async Task<SendSmsFromTemplateResponse> Handle(SendSmsFromTemplateRequest request, CancellationToken cancellationToken)
    {
        // todo: call codebook service and get notification type text
        // todo: placeholders
        var text = "todo";
        
        var notificationResult = await _repository.CreateSmsResult(
            request.Identifier?.Identity,
            request.Identifier?.IdentityScheme,
            request.CustomId,
            request.DocumentId,
            text,
            request.Phone.CountryCode,
            request.Phone.NationalNumber,
            cancellationToken);
        var notificationId = notificationResult.Id;
        
        var sendSms = new SendApi.v1.sms.SendSMS
        {
            id = notificationId.ToString(),
            phone = request.Phone.Map(),
            type = request.Type,
            text = text,
            processingPriority = request.ProcessingPriority
        };
        
        _logger.LogInformation("Sending sms from template: {sendSms}", JsonSerializer.Serialize(sendSms));

        await _mcsSmsProducer.SendSms(sendSms, cancellationToken);
        
        await _repository.UpdateResult(
            notificationId,
            NotificationState.Sent,
            token: cancellationToken);
        
        return new SendSmsFromTemplateResponse
        {
            NotificationId = notificationId
        };
    }
}