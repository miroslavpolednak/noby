using System.Text.Json;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using CIS.InternalServices.NotificationService.Msc;
using CIS.InternalServices.NotificationService.Msc.Messages;
using CIS.InternalServices.NotificationService.Msc.Messages.Dto;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.SendSms;

public class SendSmsHandler : IRequestHandler<SmsSendRequest, SmsSendResponse>
{
    // todo: replace string with Msc contract
    private readonly IProducer<Null, string> _mscSmsProducer;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<SendSmsHandler> _logger;

    public SendSmsHandler(
        IProducer<Null, string> mscSmsProducer,
        IMemoryCache memoryCache,
        ILogger<SendSmsHandler> logger)
    {
        _mscSmsProducer = mscSmsProducer;
        _memoryCache = memoryCache;
        _logger = logger;
    }
    
    public async Task<SmsSendResponse> Handle(SmsSendRequest request, CancellationToken cancellationToken)
    {
        // todo:
        _logger.LogInformation("Received request: {request}", request);
        var notificationId = Guid.NewGuid().ToString();

        _memoryCache.Set(notificationId, new ResultGetResponse
        {
            Channel = NotificationChannel.Sms,
            State = NotificationState.Sent
        });
        
        var result = await _mscSmsProducer.ProduceAsync(
            Topics.MscSenderIn,
            new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(new MscSms
                {
                    NotificationId = notificationId,
                    ProcessPriority = request.ProcessingPriority,
                    Phone = new Phone
                    {
                        CountryCode = request.Phone.CountryCode,
                        NationalPhoneNumber = request.Phone.NationalNumber
                    },
                    Text = request.Text,
                    Type = SmsNotificationType.Unknown
                })
            },
            cancellationToken);
        
        _logger.LogInformation("Received result: {result}", result);
        
        return new SmsSendResponse
        {
            NotificationId = notificationId
        };
    }
}