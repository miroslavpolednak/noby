using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<Endpoints.GetDeveloper.DeveloperItem> GetDeveloper(Endpoints.GetDeveloper.GetDeveloperRequest request, CallContext context = default);
}
