using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.SalesArrangementTypes.SalesArrangementTypeItem>> SalesArrangementTypes(Endpoints.SalesArrangementTypes.SalesArrangementTypesRequest request, CallContext context = default);
    }
}
