using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.CaseStates.CaseStateItem>> CaseStates(Endpoints.CaseStates.CaseStatesRequest request, CallContext context = default);
    }
}
