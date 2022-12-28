using System.Text.Json;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.NotificationService.Api.Services.Mcs.Mappers;
using CIS.InternalServices.NotificationService.Api.Services.Mcs.Producers;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Sms;

public class SendSmsHandler : IRequestHandler<SendSmsRequest, SendSmsResponse>
{
    private readonly McsSmsProducer _mcsSmsProducer;
    private readonly NotificationRepository _repository;
    private readonly IAuditLogger _auditLogger;
    private readonly ILogger<SendSmsHandler> _logger;

    public SendSmsHandler(
        McsSmsProducer mcsSmsProducer,
        NotificationRepository repository,
        IAuditLogger auditLogger,
        ILogger<SendSmsHandler> logger)
    {
        _mcsSmsProducer = mcsSmsProducer;
        _repository = repository;
        _auditLogger = auditLogger;
        _logger = logger;
    }
    
    public async Task<SendSmsResponse> Handle(SendSmsRequest request, CancellationToken cancellationToken)
    {
        var notificationResult = await _repository.CreateSmsResult(
            request.Identifier?.Identity,
            request.Identifier?.IdentityScheme,
            request.CustomId,
            request.DocumentId,
            request.Text,
            request.Phone.CountryCode,
            request.Phone.NationalNumber,
            cancellationToken);
        
        var notificationId = notificationResult.Id;

        var sendSms = new SendApi.v1.sms.SendSMS
        {
            id = notificationId.ToString(),
            phone = request.Phone.Map(),
            type = request.Type,
            text = request.Text,
            processingPriority = request.ProcessingPriority
        };
        
        _logger.LogInformation("Sending sms: {sendSms}", JsonSerializer.Serialize(sendSms));

        await _mcsSmsProducer.SendSms(sendSms, cancellationToken);
        
        var updateResult = await _repository.UpdateResult(
            notificationId,
            NotificationState.Sent,
            token: cancellationToken);
        
        return new SendSmsResponse
        {
            NotificationId = notificationId
        };
    }
}