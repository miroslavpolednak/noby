using ProtoBuf.Grpc;
using System;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        [Obsolete]
        Task<List<Endpoints.GetDeveloperProject.DeveloperProjectItem>> DeveloperProjects(Endpoints.DeveloperProjects.DeveloperProjectsRequest request, CallContext context = default);
    }
}