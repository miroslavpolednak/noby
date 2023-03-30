using MassTransit;

namespace DomainServices.CaseService.Api.Messaging.SendEmail;

public class SendEmailConsumer : IConsumer<cz.kb.osbs.mcs.sender.sendapi.v4.email.SendEmail>
{
    private readonly ILogger<SendEmailConsumer> _logger;

    public SendEmailConsumer(ILogger<SendEmailConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<cz.kb.osbs.mcs.sender.sendapi.v4.email.SendEmail> context)
    {
        _logger.LogInformation("Received");

        // mocking a business logic with Task.Delay
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}
