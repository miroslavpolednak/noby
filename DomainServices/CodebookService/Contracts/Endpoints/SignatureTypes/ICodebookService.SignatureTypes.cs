using DomainServices.CodebookService.Contracts.Endpoints.SignatureTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<SignatureTypeItem>> SignatureTypes(SignatureTypesRequest request, CallContext context = default);
    }
}