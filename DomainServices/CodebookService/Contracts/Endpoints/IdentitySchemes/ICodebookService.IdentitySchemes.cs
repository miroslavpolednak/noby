using DomainServices.CodebookService.Contracts.Endpoints.IdentitySchemes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<IdentitySchemeItem>> IdentitySchemes(IdentitySchemesRequest request, CallContext context = default);
    }
}
