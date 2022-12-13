using DomainServices.CodebookService.Contracts.Endpoints.IncomeOtherTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<IncomeOtherTypeItem>> IncomeOtherTypes(IncomeOtherTypesRequest request, CallContext context = default);
}