using System.Text.Json;
using CIS.Core.Exceptions;
using CIS.Infrastructure.Telemetry;
using CIS.InternalServices.NotificationService.Api.Services.Mcs.Mappers;
using CIS.InternalServices.NotificationService.Api.Services.Mcs.Producers;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.SmsNotificationTypes;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Sms;

public class SendSmsHandler : IRequestHandler<SendSmsRequest, SendSmsResponse>
{
    private readonly McsSmsProducer _mcsSmsProducer;
    private readonly NotificationRepository _repository;
    private readonly ICodebookService _codebookService;
    private readonly IAuditLogger _auditLogger;
    private readonly ILogger<SendSmsHandler> _logger;

    public SendSmsHandler(
        McsSmsProducer mcsSmsProducer,
        NotificationRepository repository,
        ICodebookService codebookService,
        IAuditLogger auditLogger,
        ILogger<SendSmsHandler> logger)
    {
        _mcsSmsProducer = mcsSmsProducer;
        _repository = repository;
        _codebookService = codebookService;
        _auditLogger = auditLogger;
        _logger = logger;
    }
    
    public async Task<SendSmsResponse> Handle(SendSmsRequest request, CancellationToken cancellationToken)
    {
        var smsTypes = await _codebookService.SmsNotificationTypes(new SmsNotificationTypesRequest(), cancellationToken);
        var smsType = smsTypes.FirstOrDefault(s => s.Code == request.Type) ?? throw new CisValidationException("todo");
        
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
            type = smsType.McsCode,
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