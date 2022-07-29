using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<GenericCodebookItem>> ObligationCorrectionTypes(Endpoints.ObligationCorrectionTypes.ObligationCorrectionTypesRequest request, CallContext context = default);
    }
}
