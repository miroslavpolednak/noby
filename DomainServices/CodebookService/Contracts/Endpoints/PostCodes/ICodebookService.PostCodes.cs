using DomainServices.CodebookService.Contracts.Endpoints.PostCodes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<PostCodeItem>> PostCodes(PostCodesRequest request, CallContext context = default);
}