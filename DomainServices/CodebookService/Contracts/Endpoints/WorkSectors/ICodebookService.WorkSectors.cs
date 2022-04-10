using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> WorkSectors(Endpoints.WorkSectors.WorkSectorsRequest request, CallContext context = default);
    }
}
