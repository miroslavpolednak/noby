using System.Text.Json;
using CIS.InternalServices.NotificationService.Api.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using CIS.InternalServices.NotificationService.Msc;
using Confluent.Kafka;
using cz.kb.osbs.mcs.sender.sendapi.v1.sms;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.SendSms;

public class SendSmsHandler : IRequestHandler<SmsSendRequest, SmsSendResponse>
{
    private readonly IProducer<Null, SendSMS> _mscSmsProducer;
    private readonly NotificationRepository _repository;
    private readonly ILogger<SendSmsHandler> _logger;

    public SendSmsHandler(
        IProducer<Null, SendSMS> mscSmsProducer,
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

        var sendSms = new SendSMS
        {
            id = notificationId,
            phone = new cz.kb.osbs.mcs.sender.sendapi.v1.Phone
            {
                countryCode = request.Phone.CountryCode,
                nationalPhoneNumber = request.Phone.NationalNumber
            },
            type = request.Type.ToString(),
            text = request.Text,
            processingPriority = request.ProcessingPriority
        };
        
        _logger.LogInformation("Sending sms: {sendSms}", JsonSerializer.Serialize(sendSms));
        
        await _mscSmsProducer.ProduceAsync(
            Topics.MscSenderIn,
            new Message<Null, SendSMS>
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