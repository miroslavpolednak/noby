using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<GenericCodebookItemWithCode>> IncomeMainTypes(Endpoints.IncomeMainTypes.IncomeMainTypesRequest request, CallContext context = default);
}