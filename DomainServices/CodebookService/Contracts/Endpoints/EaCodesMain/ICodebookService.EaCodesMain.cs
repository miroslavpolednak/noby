using DomainServices.CodebookService.Contracts.Endpoints.EaCodesMain;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<EaCodeMainItem>> EaCodesMain(EaCodesMainRequest request, CallContext context = default);
}