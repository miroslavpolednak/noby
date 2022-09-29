using System.Text.Json;
using CIS.InternalServices.NotificationService.Msc;
using CIS.InternalServices.NotificationService.Msc.Messages;
using Confluent.Kafka;

namespace Mock_KB_NotificationWorker;

public class Worker : BackgroundService
{
    // todo: replace string with Msc contract
    private readonly IConsumer<Null, string> _mscSmsConsumer;
    private readonly IProducer<Null, string> _mscResultProducer;
    private readonly ILogger<Worker> _logger;
    
    public Worker(
        IConsumer<Null, string> mscSmsConsumer,
        IProducer<Null, string> mscResultProducer,
        ILogger<Worker> logger)
    {
        _mscSmsConsumer = mscSmsConsumer;
        _mscResultProducer = mscResultProducer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _mscSmsConsumer.Subscribe(Topics.MscSenderIn);

        await Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _mscSmsConsumer.Consume(stoppingToken);
                _logger.LogInformation("Received result: {result}", result.Message.Value);

                var mscSms = JsonSerializer.Deserialize<MscSms>(result.Message.Value)!;
                
                await Task.Delay(TimeSpan.FromSeconds(8), stoppingToken); // simulation purpose
                await _mscResultProducer.ProduceAsync(
                    Topics.MscResultIn,
                    new Message<Null, string>
                    {
                        Value = JsonSerializer.Serialize(new MscResult
                        {
                            NotificationId = mscSms.NotificationId
                        })
                    },
                    stoppingToken);
            }
        }, stoppingToken);
        
        _mscSmsConsumer.Close();
    }
}