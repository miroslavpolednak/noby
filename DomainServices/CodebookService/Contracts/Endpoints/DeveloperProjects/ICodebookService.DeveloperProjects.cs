using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.DeveloperProjects.DeveloperProjectItem>> DeveloperProjects(Endpoints.DeveloperProjects.DeveloperProjectsRequest request, CallContext context = default);
    }
}