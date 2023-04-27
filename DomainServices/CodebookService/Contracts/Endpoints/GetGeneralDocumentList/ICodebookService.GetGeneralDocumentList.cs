using ProtoBuf.Grpc;
using DomainServices.CodebookService.Contracts.Endpoints.GetGeneralDocumentList;


namespace DomainServices.CodebookService.Contracts;

public partial interface ICodebookService
{
    [OperationContract]
    Task<List<GetGeneralDocumentListItem>> GetGeneralDocumentList(GetGeneralDocumentListRequest request, CallContext context = default);
}
