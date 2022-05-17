using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.Developers.DeveloperItem>> Developers(Endpoints.Developers.DevelopersRequest request, CallContext context = default);
    }
}