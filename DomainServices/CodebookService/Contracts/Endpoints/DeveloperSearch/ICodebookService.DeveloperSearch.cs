using DomainServices.CodebookService.Contracts.Endpoints.DeveloperSearch;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<DeveloperSearchItem>> DeveloperSearch(DeveloperSearchRequest request, CallContext context = default);
}