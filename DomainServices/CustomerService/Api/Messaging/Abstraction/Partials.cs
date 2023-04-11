using DomainServices.CustomerService.Api.Messaging.Abstraction;

namespace DomainServices.CustomerService.Api.Messaging.PartyCreated
{
    public partial class Party : ICustomerManagementEvent
    {
        public override string ToString() => "cz.kb.cm.be.event.partycreated.v1.PartyCreatedV1";
    }
}

namespace DomainServices.CustomerService.Api.Messaging.PartyUpdated
{
    public partial class Party : ICustomerManagementEvent
    {
        public override string ToString() => "cz.kb.cm.be.event.partyupdated.v1.PartyUpdatedV1";
    }
}