using DomainServices.CodebookService.Contracts.Endpoints.DocumentFileTypes;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<DocumentFileTypeItem>> DocumentFileTypes(DocumentFileTypesRequest request, CallContext context = default);
    }
}