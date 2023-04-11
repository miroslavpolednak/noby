using MassTransit;

namespace DomainServices.CustomerService.Api.Messaging.PartyCreated;

public class PartyCreatedConsumer : IConsumer<Party>
{
    public Task Consume(ConsumeContext<Party> context)
    {
        var message = context.Message;
        return Task.CompletedTask;
    }
}