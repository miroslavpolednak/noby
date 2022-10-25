﻿using System.Text.Json;
using CIS.InternalServices.NotificationService.Api.Mappers;
using CIS.InternalServices.NotificationService.Api.Messaging.Producers;
using CIS.InternalServices.NotificationService.Api.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.Handlers;

public class SendSmsFromTemplateHandler : IRequestHandler<SmsFromTemplateSendRequest, SmsFromTemplateSendResponse>
{
    private readonly LogmanSmsProducer _logmanSmsProducer;
    private readonly NotificationRepository _repository;
    private readonly ILogger<SendSmsFromTemplateHandler> _logger;
    
    public SendSmsFromTemplateHandler(
        LogmanSmsProducer logmanSmsProducer,
        NotificationRepository repository,
        ILogger<SendSmsFromTemplateHandler> logger)
    {
        _logmanSmsProducer = logmanSmsProducer;
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<SmsFromTemplateSendResponse> Handle(SmsFromTemplateSendRequest request, CancellationToken cancellationToken)
    {
        var notificationResult = await _repository.CreateResult(NotificationChannel.Sms, cancellationToken);
        var notificationId = notificationResult.Id;
        
        // todo: call codebook service and get notification type text
        // todo: placeholders
        var sendSms = new SendApi.v1.sms.SendSMS
        {
            id = notificationId.ToString(),
            phone = request.Phone.Map(),
            type = request.Type.ToString(),
            text = "todo",
            processingPriority = request.ProcessingPriority
        };
        
        _logger.LogInformation("Sending sms from template: {sendSms}", JsonSerializer.Serialize(sendSms));

        await _logmanSmsProducer.SendSms(sendSms, cancellationToken);
        
        await _repository.UpdateResult(
            notificationId,
            NotificationState.Sent,
            new HashSet<string>(),
            cancellationToken);
        
        return new SmsFromTemplateSendResponse
        {
            NotificationId = notificationId
        };
    }
}