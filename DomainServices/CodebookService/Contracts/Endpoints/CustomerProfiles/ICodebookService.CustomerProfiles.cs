using DomainServices.CodebookService.Contracts.Endpoints.CustomerProfiles;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<CustomerProfileItem>> CustomerProfiles(CustomerProfilesRequest request, CallContext context = default);
    }
}