using DomainServices.CodebookService.Contracts.Endpoints;
using DomainServices.CodebookService.Contracts.Endpoints.StatementSubscriptionTypes;
using ProtoBuf.Grpc;
namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<GenericCodebookItemWithCodeAndDefault>> StatementSubscriptionTypes(StatementSubscriptionTypesRequest request, CallContext context = default);
}
