using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> ProductLoanPurposes(Endpoints.ProductLoanPurposes.ProductLoanPurposesRequest request, CallContext context = default);
    }
}
