using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.DrawingDurations.DrawingDurationItem>> DrawingDurations(Endpoints.DrawingDurations.DrawingDurationsRequest request, CallContext context = default);
    }
}