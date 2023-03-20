using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<Endpoints.GetDeveloperProject.DeveloperProjectItem> GetDeveloperProject(Endpoints.GetDeveloperProject.GetDeveloperProjectRequest request, CallContext context = default);
}
