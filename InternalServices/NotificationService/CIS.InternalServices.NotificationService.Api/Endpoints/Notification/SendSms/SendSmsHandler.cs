using System.Text.Json;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using CIS.InternalServices.NotificationService.Msc;
using Confluent.Kafka;
using cz.kb.osbs.mcs.sender.sendapi.v1.sms;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.SendSms;

public class SendSmsHandler : IRequestHandler<SmsSendRequest, SmsSendResponse>
{
    private readonly IProducer<Null, SendSMS> _mscSmsProducer;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<SendSmsHandler> _logger;

    public SendSmsHandler(
        IProducer<Null, SendSMS> mscSmsProducer,
        IMemoryCache memoryCache,
        ILogger<SendSmsHandler> logger)
    {
        _mscSmsProducer = mscSmsProducer;
        _memoryCache = memoryCache;
        _logger = logger;
    }
    
    public async Task<SmsSendResponse> Handle(SmsSendRequest request, CancellationToken cancellationToken)
    {
        var notificationId = Guid.NewGuid().ToString();

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
        
        _memoryCache.Set(notificationId, new ResultGetResponse
        {
            NotificationId = notificationId,
            Channel = NotificationChannel.Sms,
            State = NotificationState.Sent
        });
        
        _logger.LogInformation("Sending sms: {sendSms}", JsonSerializer.Serialize(sendSms));
        
        await _mscSmsProducer.ProduceAsync(
            Topics.MscSenderIn,
            new Message<Null, SendSMS>
            {
                Value = sendSms
            },
            cancellationToken);
        
        return new SmsSendResponse
        {
            NotificationId = notificationId
        };
    }
}