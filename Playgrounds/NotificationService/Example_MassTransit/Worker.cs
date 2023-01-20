using cz.kb.osbs.mcs.sender.sendapi.v4;
using cz.kb.osbs.mcs.sender.sendapi.v4.email;
using MassTransit;

namespace Example_MassTransit;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _provider;

    public Worker(ILogger<Worker> logger, IServiceProvider provider)
    {
        _logger = logger;
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _provider.CreateScope();
        var producer = scope.ServiceProvider.GetRequiredService<ITopicProducer<SendEmail>>();
        
        await Task.Delay(4000, stoppingToken);

        var id = Guid.NewGuid().ToString();
        
        // https://wiki.kb.cz/display/BST/Seznam+konzumentu
        var consumerId = "HF_STARBUILD";
        
        _logger.LogInformation("Sending Email with id = {0}", id);
        
        await producer.Produce(new SendEmail()
        {
            id = id,
            sender = new EmailAddress
            {
                value = "notification-service@kb.cz",
            },
            to = new List<EmailAddress>(){ new () {value = "zdenek_siblik@kb.cz" } },
            content = new Content
            {
                charset = "UTF-8",
                text = "Testovací email přes MCS"
            },
            subject = "MCS Testovací email",
            notificationConsumer = new NotificationConsumer
            { 
                consumerId = consumerId
            }
        }, new ProducerPipe<SendEmail>(
            Guid.NewGuid().ToString(),
            Topics.McsResult, 
            "kafkabc-test-broker.service.ist.consul-nprod.kb.cz",
            DateTime.Now), stoppingToken);
        
        _logger.LogInformation("Email with id = {0} sent", id);
    }
}