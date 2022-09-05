using DomainServices.CodebookService.Contracts.Endpoints.ObligationLaExposures;
using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<ObligationLaExposureItem>> ObligationLaExposures(ObligationLaExposuresRequest request, CallContext context = default);
    }
}
