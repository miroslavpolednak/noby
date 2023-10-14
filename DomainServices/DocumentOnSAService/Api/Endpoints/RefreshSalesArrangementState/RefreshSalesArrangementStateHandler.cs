using DomainServices.DocumentOnSAService.Api.Common;
using DomainServices.DocumentOnSAService.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.DocumentOnSAService.Api.Endpoints.RefreshSalesArrangementState;

public class RefreshSalesArrangementStateHandler : IRequestHandler<RefreshSalesArrangementStateRequest, Empty>
{
    private readonly ISalesArrangementStateManager _salesArrangementStateManager;

    public RefreshSalesArrangementStateHandler(ISalesArrangementStateManager salesArrangementStateManager)
    {
        _salesArrangementStateManager = salesArrangementStateManager;
    }

    public async Task<Empty> Handle(RefreshSalesArrangementStateRequest request, CancellationToken cancellationToken)
    {
        await _salesArrangementStateManager.SetSalesArrangementStateAccordingDocumentsOnSa(request.SalesArrangementId, cancellationToken);
        return new Empty();
    }
}
