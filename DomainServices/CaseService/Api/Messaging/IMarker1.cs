using Avro.Specific;
using DomainServices.CaseService.Api.Messaging;

namespace DomainServices.CaseService.Api.Messaging
{
    public interface IMarker1 : ISpecificRecord
    {
    }
}

namespace cz.kb.osbs.mcs.sender.sendapi.v4
{
    public partial class SendEmail : IMarker1
    {
    }
}