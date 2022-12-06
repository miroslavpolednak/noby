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
        
        await Task.Delay(10000, stoppingToken);
        
        await producer.Produce(new SendEmail()
        {
            id = Guid.NewGuid().ToString(),
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
            subject = "MCS Testovací email"
        }, cancellationToken: stoppingToken);
    }
}