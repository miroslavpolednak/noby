using MassTransit;

namespace DomainServices.CustomerService.Api.Messaging.PartyCreated;

public class PartyCreatedConsumer : IConsumer<PartyCreatedV1>
{
    public Task Consume(ConsumeContext<PartyCreatedV1> context)
    {
        var message = context.Message;
        return Task.CompletedTask;
    }
}