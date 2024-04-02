using CIS.Core;
using DomainServices.CaseService.Clients.v1;
using DomainServices.OfferService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;

internal sealed class GetMortgageRefixationHandler
    : IRequestHandler<GetMortgageRefixationRequest, GetMortgageRefixationResponse>
{
    public async Task<GetMortgageRefixationResponse> Handle(GetMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        // vsechny tasky z WF, potom vyfiltrovat jen na konkretni processId
        var tasks = (await _caseService.GetTaskList(request.CaseId, cancellationToken))
            .Where(t => t.ProcessId == request.ProcessId)
            .ToList();

        // toto je aktivni task!
        bool existActiveIC = tasks.Any(t => t.TaskTypeId == (int)WorkflowTaskTypes.PriceException && !t.Cancelled && t.DecisionId != 2 && t.PhaseTypeId == 2);

        var response = new GetMortgageRefixationResponse
        {
            Offers = await getOfferList(request.CaseId, cancellationToken),
            Tasks = (await tasks
                .SelectAsync(t => _workflowMapper.MapTask(t, cancellationToken)))
                .ToList()
        };

        throw new NotImplementedException();
    }

    /// <summary>
    /// Seznam nabidek
    /// </summary>
    private async Task<List<RefixationOfferDetail>> getOfferList(long caseId, CancellationToken cancellationToken)
    {
        var offers = await _offerService.GetOfferList(caseId, DomainServices.OfferService.Contracts.OfferTypes.MortgageRefixation, false, cancellationToken);

        return offers.Select(offer =>
        {
            var flags = (OfferFlagTypes)offer.Data.Flags;

            return new RefixationOfferDetail
            {
                OfferId = offer.Data.OfferId,
                IsCommunicated = flags.HasFlag(OfferFlagTypes.Communicated),
                IsCurrent = flags.HasFlag(OfferFlagTypes.Current),
                IsLegalNotice = flags.HasFlag(OfferFlagTypes.LegalNotice),
                IsLiked = flags.HasFlag(OfferFlagTypes.Liked),
                IsSelected = flags.HasFlag(OfferFlagTypes.Selected),
                FixedRatePeriod = offer.MortgageRefixation.SimulationInputs.FixedRatePeriod,
                InterestRate = offer.MortgageRefixation.SimulationInputs.InterestRate,
                InterestRateDiscount = offer.MortgageRefixation.SimulationInputs.InterestRateDiscount,
                LoanPaymentAmount = offer.MortgageRefixation.SimulationResults.LoanPaymentAmount,
                LoanPaymentAmountDiscounted = offer.MortgageRefixation.SimulationResults.LoanPaymentAmountDiscounted
            };
        }).ToList();
    }

    private readonly Services.WorkflowMapper.IWorkflowMapperService _workflowMapper;
    private readonly IOfferServiceClient _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICaseServiceClient _caseService;

    public GetMortgageRefixationHandler(ISalesArrangementServiceClient salesArrangementService, ICaseServiceClient caseService, IOfferServiceClient offerService, Services.WorkflowMapper.IWorkflowMapperService workflowMapper)
    {
        _salesArrangementService = salesArrangementService;
        _caseService = caseService;
        _offerService = offerService;
        _workflowMapper = workflowMapper;
    }
}
