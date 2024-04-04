using CIS.Core;
using DomainServices.CaseService.Clients.v1;
using DomainServices.OfferService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.Offer.SimulateMortgageRefixationOfferList;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;

internal sealed class GetMortgageRefixationHandler
    : IRequestHandler<GetMortgageRefixationRequest, GetMortgageRefixationResponse>
{
    public async Task<GetMortgageRefixationResponse> Handle(GetMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        GetMortgageRefixationResponse response = new();
        decimal? icRate = null;

        if (request.ProcessId.HasValue)
        {
            await fillComments(response, request.CaseId, request.ProcessId.Value, cancellationToken);

            // vsechny tasky z WF, potom vyfiltrovat jen na konkretni processId
            var tasks = (await _caseService.GetTaskList(request.CaseId, cancellationToken))
                .Where(t => t.ProcessId == request.ProcessId)
                .ToList();
            
            response.Tasks = (await tasks
                .SelectAsync(t => _workflowMapper.MapTask(t, cancellationToken)))
                .ToList();

            // toto je aktivni task!
            var activeIC = tasks.FirstOrDefault(t => t.TaskTypeId == (int)WorkflowTaskTypes.PriceException && !t.Cancelled && t.DecisionId != 2 && t.PhaseTypeId == 2);
            
            // jestlize existuje aktivni IC
            if (activeIC is not null)
            {
                var taskDetail = await _caseService.GetTaskDetail(activeIC.TaskIdSb, cancellationToken);
                icRate = taskDetail.TaskDetail.PriceException?.LoanInterestRate?.LoanInterestRateDiscount;
            }
        }

        response.Offers = (await _mediator.Send(new SimulateMortgageRefixationOfferListRequest
        {
            CaseId = request.CaseId,
            InterestRateDiscount = icRate
        }, cancellationToken))
            .Offers;

        response.ContainsInconsistentIndividualPriceData = !(response.Offers?.All(t => t.InterestRateDiscount == icRate) ?? true);

        return response;
    }

    private async Task fillComments(GetMortgageRefixationResponse response, long caseId, long processId, CancellationToken cancellationToken)
    {
        var allSalesArrangements = await _salesArrangementService.GetSalesArrangementList(caseId, cancellationToken);
        var salesArrangement = allSalesArrangements.SalesArrangements.FirstOrDefault(t => t.TaskProcessId == processId);

        response.IndividualPriceCommentLastVersion = salesArrangement?.Retention?.IndividualPriceCommentLastVersion;
        response.Comment = salesArrangement?.Retention?.Comment;
    }

    private readonly IMediator _mediator;
    private readonly Services.WorkflowMapper.IWorkflowMapperService _workflowMapper;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;

    public GetMortgageRefixationHandler(ISalesArrangementServiceClient salesArrangementService, ICaseServiceClient caseService, Services.WorkflowMapper.IWorkflowMapperService workflowMapper, IMediator mediator)
    {
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _workflowMapper = workflowMapper;
        _mediator = mediator;
    }
}
