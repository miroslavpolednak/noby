using DomainServices.CodebookService.Contracts.Endpoints.CustomerRoles;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<CustomerRoleItem>> CustomerRoles(CustomerRolesRequest request, CallContext context = default);
    }
}