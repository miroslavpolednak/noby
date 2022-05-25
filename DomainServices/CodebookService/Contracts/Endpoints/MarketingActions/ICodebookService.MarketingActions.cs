using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.MarketingActions.MarketingActionItem>> MarketingActions(Endpoints.MarketingActions.MarketingActionsRequest request, CallContext context = default);
    }
}
