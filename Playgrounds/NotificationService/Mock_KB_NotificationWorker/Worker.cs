using System.Text.Json;
using CIS.InternalServices.NotificationService.Mcs;
using Confluent.Kafka;
using cz.kb.osbs.mcs.notificationreport.eventapi.v2.notificationreport;
using cz.kb.osbs.mcs.notificationreport.eventapi.v2.report;
using cz.kb.osbs.mcs.sender.sendapi.v1.sms;

namespace Mock_KB_NotificationWorker;

public class Worker : BackgroundService
{
    private readonly IConsumer<string, SendSMS> _mscSmsConsumer;
    private readonly IProducer<string, NotificationReport> _mscResultProducer;
    private readonly ILogger<Worker> _logger;
    
    public Worker(
        IConsumer<string, SendSMS> mscSmsConsumer,
        IProducer<string, NotificationReport> mscResultProducer,
        ILogger<Worker> logger)
    {
        _mscSmsConsumer = mscSmsConsumer;
        _mscResultProducer = mscResultProducer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _mscSmsConsumer.Subscribe(Topics.McsSenderIn);

        await Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _mscSmsConsumer.Consume(stoppingToken);
                var sendSms = result.Message.Value;
                _logger.LogInformation("Received sendSms: {sendSms}", JsonSerializer.Serialize(sendSms));

                // simulation purpose 16 seconds
                await Task.Delay(TimeSpan.FromSeconds(16), stoppingToken);
                
                await _mscResultProducer.ProduceAsync(
                    Topics.McsResultIn,
                    new Message<string, NotificationReport>
                    {
                        Key = "",
                        Value = new NotificationReport
                        {
                            id = sendSms.id,
                            channel = new Channel
                            {
                                id = "sms",
                            },
                            state = "ok",
                            exactlyOn = DateTime.Now,
                            finalState = true,
                            notificationErrors = new List<NotificationError>()
                        }
                    }, 
                    stoppingToken);
            }
        }, stoppingToken);
        
        _mscSmsConsumer.Close();
    }
}