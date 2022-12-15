using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<Endpoints.ProfessionTypes.ProfessionTypeItem>> ProfessionTypes(Endpoints.ProfessionTypes.ProfessionTypesRequest request, CallContext context = default);
}