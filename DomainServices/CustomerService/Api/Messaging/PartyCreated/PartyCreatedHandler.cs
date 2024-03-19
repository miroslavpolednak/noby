using cz.kb.cm.be.@event.partycreated.v1.PartyCreatedV1;
using KafkaFlow;

namespace DomainServices.CustomerService.Api.Messaging.PartyCreated;

internal class PartyCreatedHandler : IMessageHandler<PartyCreatedV1>
{
    public Task Handle(IMessageContext context, PartyCreatedV1 message)
    {
        return Task.CompletedTask;
    }
}