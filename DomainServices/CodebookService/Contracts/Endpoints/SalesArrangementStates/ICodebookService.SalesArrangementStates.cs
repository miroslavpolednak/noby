using DomainServices.CodebookService.Contracts.Endpoints.SalesArrangementStates;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<SalesArrangementStateItem>> SalesArrangementStates(SalesArrangementStatesRequest request, CallContext context = default);
    }
}
