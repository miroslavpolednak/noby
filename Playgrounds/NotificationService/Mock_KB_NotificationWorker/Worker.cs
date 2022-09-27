using CIS.InternalServices.NotificationService.Msc;
using CIS.InternalServices.NotificationService.Msc.Messages;
using Confluent.Kafka;

namespace Mock_KB_NotificationWorker;

public class Worker : BackgroundService
{
    private readonly IConsumer<Null, MscSms> _mscSmsConsumer;
    private readonly IProducer<Null, MscResult> _mscResultProducer;
    private readonly ILogger<Worker> _logger;
    
    public Worker(
        IConsumer<Null, MscSms> mscSmsConsumer,
        IProducer<Null, MscResult> mscResultProducer,
        ILogger<Worker> logger)
    {
        _mscSmsConsumer = mscSmsConsumer;
        _mscResultProducer = mscResultProducer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _mscSmsConsumer.Subscribe(Topics.MscSenderIn);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var result = _mscSmsConsumer.Consume(stoppingToken);
            _logger.LogInformation("Received result: {result}", result.Message.Value);
            
            await _mscResultProducer.ProduceAsync(
                Topics.MscResultIn, 
                new Message<Null, MscResult>
                {
                    Value = new MscResult
                    {
                        NotificationId = result.Message.Value.NotificationId
                    }
                },
                stoppingToken);
        }
        
        _mscSmsConsumer.Unsubscribe();
    }
}