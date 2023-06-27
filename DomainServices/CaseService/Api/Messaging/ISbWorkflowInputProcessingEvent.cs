using Avro.Specific;
using DomainServices.CaseService.Api.Messaging;

namespace DomainServices.CaseService.Api.Messaging
{
    public interface ISbWorkflowInputProcessingEvent : ISpecificRecord
    {
    }
}

namespace cz.mpss.api.starbuild.mortgageworkflow.mortgageinputprocessingevents.v1
{
    public partial class CaseStateChanged_ProcessingCompleted : ISbWorkflowInputProcessingEvent
    {
    }
}
