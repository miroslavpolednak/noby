using Avro.Specific;
using DomainServices.CaseService.Api.Messaging;

namespace DomainServices.CaseService.Api.Messaging
{
    internal interface IMarkerTest : ISpecificRecord
    {
    }
}

namespace cz.kb.osbs.mcs.sender.sendapi.v4.email
{
    public partial class SendEmail : IMarkerTest
    {
    }
}

namespace cz.kb.osbs.mcs.notificationreport.eventapi.v3.report
{
    public partial class NotificationReport : IMarkerTest
    {
    }
}
