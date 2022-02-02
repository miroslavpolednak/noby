using DomainServices.CodebookService.Contracts.Endpoints.ProductLoanPurposes;
using ProtoBuf.Grpc;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<ProductLoanPurposesItem>> ProductLoanPurposes(ProductLoanPurposesRequest request, CallContext context = default);
    }
}
