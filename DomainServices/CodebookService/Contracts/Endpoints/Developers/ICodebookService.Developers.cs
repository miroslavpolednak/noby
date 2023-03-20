using ProtoBuf.Grpc;
using System;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        [Obsolete]
        Task<List<Endpoints.GetDeveloper.DeveloperItem>> Developers(Endpoints.Developers.DevelopersRequest request, CallContext context = default);
    }
}