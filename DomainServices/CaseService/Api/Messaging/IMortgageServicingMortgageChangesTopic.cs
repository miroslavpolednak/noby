using Avro.Specific;
using DomainServices.CaseService.Api.Messaging;

namespace DomainServices.CaseService.Api.Messaging
{
    public interface IMortgageServicingMortgageChangesTopic : ISpecificRecord
    {
    }
}

namespace cz.kb.api.mortgageservicingevents.v2
{
    public partial class MortgageInstanceChanged : IMortgageServicingMortgageChangesTopic
    {
    }
}