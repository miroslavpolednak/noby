using DomainServices.CodebookService.Contracts.Endpoints.StatementTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<StatementTypeItem>> StatementTypes(StatementTypesRequest request, CallContext context = default);
}