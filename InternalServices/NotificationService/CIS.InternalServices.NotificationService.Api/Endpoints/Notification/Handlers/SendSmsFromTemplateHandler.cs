using System.Text.Json;
using CIS.InternalServices.NotificationService.Api.Mappers;
using CIS.InternalServices.NotificationService.Api.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using CIS.InternalServices.NotificationService.Msc;
using Confluent.Kafka;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.Handlers;

public class SendSmsFromTemplateHandler : IRequestHandler<SmsFromTemplateSendRequest, SmsFromTemplateSendResponse>
{
    private readonly IProducer<Null, SendApi.v1.sms.SendSMS> _mscSmsProducer;
    private readonly NotificationRepository _repository;
    private readonly ILogger<SendSmsFromTemplateHandler> _logger;
    
    public SendSmsFromTemplateHandler(
        IProducer<Null, SendApi.v1.sms.SendSMS> mscSmsProducer,
        NotificationRepository repository,
        ILogger<SendSmsFromTemplateHandler> logger)
    {
        _mscSmsProducer = mscSmsProducer;
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<SmsFromTemplateSendResponse> Handle(SmsFromTemplateSendRequest request, CancellationToken cancellationToken)
    {
        var notificationResult = await _repository.CreateResult(NotificationChannel.Sms, cancellationToken);
        var notificationId = notificationResult.Id.ToString();
        
        // todo: call codebook service and get notification type text
        // todo: placeholders
        var sendSms = new SendApi.v1.sms.SendSMS
        {
            id = notificationId,
            phone = request.Phone.Map(),
            type = request.Type.ToString(),
            text = "todo",
            processingPriority = request.ProcessingPriority
        };
        
        _logger.LogInformation("Sending sms from template: {sendSms}", JsonSerializer.Serialize(sendSms));
        
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
        
        return new SmsFromTemplateSendResponse
        {
            NotificationId = notificationId
        };
    }
}