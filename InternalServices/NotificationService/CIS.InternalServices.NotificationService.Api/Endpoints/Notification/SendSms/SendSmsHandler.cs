using CIS.InternalServices.NotificationService.Contracts.Sms;
using CIS.InternalServices.NotificationService.Msc;
using CIS.InternalServices.NotificationService.Msc.Messages;
using CIS.InternalServices.NotificationService.Msc.Messages.Dto;
using Confluent.Kafka;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.SendSms;

public class SendSmsHandler : IRequestHandler<SmsSendRequest, SmsSendResponse>
{
    private readonly IProducer<Null, MscSms> _mscSmsProducer;
    private readonly ILogger<SendSmsHandler> _logger;

    public SendSmsHandler(
        IProducer<Null, MscSms> mscSmsProducer,
        ILogger<SendSmsHandler> logger)
    {
        _mscSmsProducer = mscSmsProducer;
        _logger = logger;
    }
    
    public async Task<SmsSendResponse> Handle(SmsSendRequest request, CancellationToken cancellationToken)
    {
        // todo:
        _logger.LogInformation("Received request: {request}", request);
        var notificationId = Guid.NewGuid().ToString();
        
        var result = await _mscSmsProducer.ProduceAsync(
            Topics.MscSenderIn,
            new Message<Null, MscSms>
            {
                Value = new MscSms
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
                }
            },
            cancellationToken);
        
        _logger.LogInformation("Received result: {result}", result);
        
        return new SmsSendResponse
        {
            NotificationId = notificationId
        };
    }
}