using MassTransit;

namespace DomainServices.CustomerService.Api.Messaging.PartyUpdated;

public class PartyUpdatedConsumer : IConsumer<PartyUpdatedV1>
{
    // HouseholdService - CustomerOnSAService.cs
    public Task Consume(ConsumeContext<PartyUpdatedV1> context)
    {
        var message = context.Message;
        return Task.CompletedTask;
    }
}