using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<Endpoints.GetOperator.GetOperatorItem> GetOperator(Endpoints.GetOperator.GetOperatorRequest request, CallContext context = default);
}
