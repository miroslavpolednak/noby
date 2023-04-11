using MassTransit;

namespace DomainServices.CustomerService.Api.Messaging.PartyUpdated;

public class PartyUpdatedConsumer : IConsumer<Party>
{
    public Task Consume(ConsumeContext<Party> context)
    {
        var message = context.Message;
        return Task.CompletedTask;
    }
}