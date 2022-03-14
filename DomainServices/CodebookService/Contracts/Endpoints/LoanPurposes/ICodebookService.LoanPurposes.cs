using DomainServices.CodebookService.Contracts.Endpoints.LoanPurposes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<LoanPurposesItem>> LoanPurposes(LoanPurposesRequest request, CallContext context = default);
    }
}
