using System.Text.Json;
using CIS.InternalServices.NotificationService.Api.Services.Mcs.Mappers;
using CIS.InternalServices.NotificationService.Api.Services.Mcs.Producers;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Sms;

public class SendSmsHandler : IRequestHandler<SmsSendRequest, SmsSendResponse>
{
    private readonly LogmanSmsProducer _logmanSmsProducer;
    private readonly NotificationRepository _repository;
    private readonly ILogger<SendSmsHandler> _logger;

    public SendSmsHandler(
        LogmanSmsProducer logmanSmsProducer,
        NotificationRepository repository,
        ILogger<SendSmsHandler> logger)
    {
        _logmanSmsProducer = logmanSmsProducer;
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<SmsSendResponse> Handle(SmsSendRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        var notificationResult = await _repository.CreateResult(NotificationChannel.Sms, cancellationToken);
        var notificationId = notificationResult.Id;

        var sendSms = new SendApi.v1.sms.SendSMS
        {
            id = notificationId.ToString(),
            phone = request.Phone.Map(),
            type = request.Type.ToString(),
            text = request.Text,
            processingPriority = request.ProcessingPriority
        };
        
        _logger.LogInformation("Sending sms: {sendSms}", JsonSerializer.Serialize(sendSms));

        var sendResult = await _logmanSmsProducer.SendSms(sendSms, cancellationToken);
        
        var updateResult = await _repository.UpdateResult(
            notificationId,
            NotificationState.Sent,
            new HashSet<string>(),
            cancellationToken);
        
        return new SmsSendResponse
        {
            NotificationId = notificationId
        };
    }
}