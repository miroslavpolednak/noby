using Avro.Specific;
using DomainServices.RealEstateValuationService.Api.Messaging;

namespace DomainServices.RealEstateValuationService.Api.Messaging
{
    public interface ISbWorkflowProcessEvent : ISpecificRecord
    {
    }
}

namespace cz.mpss.api.starbuild.mortgageworkflow.mortgageprocessevents.v1
{
    public partial class CollateralValuationProcessChanged : ISbWorkflowProcessEvent
    {
    }
    
    public partial class InformationRequestProcessChanged : ISbWorkflowProcessEvent
    {
    }
}