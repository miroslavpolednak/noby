using Avro.Specific;
using DomainServices.CaseService.Api.Messaging;

namespace DomainServices.CaseService.Api.Messaging
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
    
    public partial class ConsultationRequestProcessChanged : ISbWorkflowProcessEvent
    {
    }
    
    public partial class IndividualPricingProcessChanged : ISbWorkflowProcessEvent
    {
    }
    
    public partial class InformationRequestProcessChanged : ISbWorkflowProcessEvent
    {
    }
    
    public partial class MainLoanProcessChanged : ISbWorkflowProcessEvent
    {
    }
    
    public partial class WithdrawalProcessChanged : ISbWorkflowProcessEvent
    {
    }
}