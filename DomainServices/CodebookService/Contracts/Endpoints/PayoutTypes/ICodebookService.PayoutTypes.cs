using DomainServices.CodebookService.Contracts.Endpoints.PayoutTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<PayoutTypeItem>> PayoutTypes(PayoutTypesRequest request, CallContext context = default);
}
