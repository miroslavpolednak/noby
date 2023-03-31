using Avro.Specific;
using DomainServices.CaseService.Api.Messaging;

namespace DomainServices.CaseService.Api.Messaging
{
    public interface IMarker1 : ISpecificRecord
    {
    }
}

namespace cz.mpss.api.starbuild.mortgage.workflow.processevents.v1
{
    public partial class MainLoanProcessChanged : IMarker1
    {
    }
}
