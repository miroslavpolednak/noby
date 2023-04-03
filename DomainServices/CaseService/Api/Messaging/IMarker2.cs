using Avro.Specific;
using DomainServices.CaseService.Api.Messaging;

namespace DomainServices.CaseService.Api.Messaging
{
    public interface IMarker2 : ISpecificRecord
    {
    }
}

namespace cz.mpss.api.starbuild.mortgage.workflow.inputprocessingevents.v1
{
    public partial class CaseStateChanged_ProcessingCompleted : IMarker2
    {
    }
}
