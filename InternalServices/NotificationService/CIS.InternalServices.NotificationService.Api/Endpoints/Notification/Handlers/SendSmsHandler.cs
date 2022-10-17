using System.Text.Json;
using CIS.InternalServices.NotificationService.Api.Mappers;
using CIS.InternalServices.NotificationService.Api.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using CIS.InternalServices.NotificationService.Msc;
using Confluent.Kafka;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.Handlers;

public class SendSmsHandler : IRequestHandler<SmsSendRequest, SmsSendResponse>
{
    private readonly IProducer<Null, SendApi.v1.sms.SendSMS> _mscSmsProducer;
    private readonly NotificationRepository _repository;
    private readonly ILogger<SendSmsHandler> _logger;

    public SendSmsHandler(
        IProducer<Null, SendApi.v1.sms.SendSMS> mscSmsProducer,
        NotificationRepository repository,
        ILogger<SendSmsHandler> logger)
    {
        _mscSmsProducer = mscSmsProducer;
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<SmsSendResponse> Handle(SmsSendRequest request, CancellationToken cancellationToken)
    {
        var notificationResult = await _repository.CreateResult(NotificationChannel.Sms, cancellationToken);
        var notificationId = notificationResult.Id.ToString();

        var sendSms = new SendApi.v1.sms.SendSMS
        {
            id = notificationId,
            phone = request.Phone.Map(),
            type = request.Type.ToString(),
            text = request.Text,
            processingPriority = request.ProcessingPriority
        };
        
        _logger.LogInformation("Sending sms: {sendSms}", JsonSerializer.Serialize(sendSms));
        
        await _mscSmsProducer.ProduceAsync(
            Topics.MscSenderIn,
            new Message<Null, SendApi.v1.sms.SendSMS>
            {
                Value = sendSms
            },
            cancellationToken);

        await _repository.UpdateResult(
            notificationResult.Id,
            NotificationState.Sent,
            new HashSet<string>(),
            cancellationToken);
        
        return new SmsSendResponse
        {
            NotificationId = notificationId
        };
    }
}