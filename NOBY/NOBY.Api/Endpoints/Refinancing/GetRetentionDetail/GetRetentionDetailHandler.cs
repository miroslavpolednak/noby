using DomainServices.CaseService.Clients.v1;
using DomainServices.OfferService.Clients;
using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Refinancing.GetRetentionDetail;

internal sealed class GetRetentionDetailHandler
    : IRequestHandler<GetRetentionDetailRequest, GetRetentionDetailResponse>
{
    public async Task<GetRetentionDetailResponse> Handle(GetRetentionDetailRequest request, CancellationToken cancellationToken)
    {
        // zjistit refinancingState
        var refinancingState = await getRefinancingStateId(request.CaseId, request.ProcessId, cancellationToken);

        if (refinancingState is (RefinancingStates.Zruseno or RefinancingStates.Dokonceno))
        {
            throw new NobyValidationException(90032, $"RefinancingState is not allowed: {refinancingState}");
        }

        // vsechny tasky z WF, potom vyfiltrovat jen na konkretni processId
        var tasks = (await _caseService.GetTaskList(request.CaseId, cancellationToken))
            .Where(t => t.ProcessId == request.ProcessId)
            .ToList();

        if (refinancingState == RefinancingStates.RozpracovanoVNoby)
        {
            var offer = await _offerService.GetOffer(1, cancellationToken);

            return new GetRetentionDetailResponse
            {
                IsReadonly = false
            };
        }
        else
        {
            var mortgage = (await _productService.GetMortgage(request.CaseId, cancellationToken)).Mortgage;

            return new GetRetentionDetailResponse
            {
                IsReadonly = true
            };
        }
    }

    private async Task<RefinancingStates> getRefinancingStateId(long caseId, long processId, CancellationToken cancellationToken)
    {
        var allSalesArrangements = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);

        var currentProcessSA = allSalesArrangements.SalesArrangements.FirstOrDefault(t => t.TaskProcessId == processId);
        if (currentProcessSA is not null)
        {
            var currentProcessSADetail = await _salesArrangementService.GetSalesArrangement(currentProcessSA.SalesArrangementId, cancellationToken);
            if (currentProcessSA.Retention?.ManagedByRC2 ?? false)
            {
                // ref.state staci vzit pouze z SA
                return RefinancingHelper.GetRefinancingState((SalesArrangementStates)currentProcessSA.State);
            }
        }

        // ziskat detail procesu
        var processes = await _caseService.GetProcessList(caseId, cancellationToken);
        var process = processes.FirstOrDefault(t => t.ProcessId == processId)
            ?? throw new NobyValidationException($"ProcessId {processId} not found in GetProcessList");

        if (process.ProcessTypeId != 3 || process.RefinancingProcess?.RefinancingType != 1)
        {
            throw new NobyValidationException(90032, "ProcessTypeId!=3 or RefinancingType!=1");
        }

        return RefinancingHelper.GetRefinancingState(currentProcessSA?.Retention?.ManagedByRC2 ?? false, currentProcessSA?.TaskProcessId, process);
    }

    private readonly IOfferServiceClient _offerService;
    private readonly IProductServiceClient _productService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;

    public GetRetentionDetailHandler(ISalesArrangementServiceClient salesArrangementService, ICaseServiceClient caseService, IProductServiceClient productService, IOfferServiceClient offerService)
    {
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _productService = productService;
        _offerService = offerService;
    }
}
