using DomainServices.CaseService.Clients.v1;
using NOBY.Api.Endpoints.Offer.SimulateMortgageRefixationOfferList;
using NOBY.Services.MortgageRefinancing;

namespace NOBY.Api.Endpoints.Refinancing.GetMortgageRefixation;

internal sealed class GetMortgageRefixationHandler(
    ICaseServiceClient _caseService,
    MortgageRefinancingWorkflowService _refinancingWorkflowService,
    IMediator _mediator, 
    Services.ResponseCodes.ResponseCodesService _responseCodes)
        : IRequestHandler<GetMortgageRefixationRequest, GetMortgageRefixationResponse>
{
    public async Task<GetMortgageRefixationResponse> Handle(GetMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        var retentionData = await _refinancingWorkflowService.GetRefinancingData(request.CaseId, request.ProcessId, RefinancingTypes.MortgageRefixation, cancellationToken);

        GetMortgageRefixationResponse response = new()
        {
            ResponseCodes = await _responseCodes.GetMortgageResponseCodes(request.CaseId, DomainServices.OfferService.Contracts.OfferTypes.MortgageRefixation, cancellationToken),
            IsReadOnly = retentionData.RefinancingState == RefinancingStates.RozpracovanoVNoby,
            Tasks = retentionData.Tasks,
            IndividualPriceCommentLastVersion = retentionData.SalesArrangement?.Refixation?.IndividualPriceCommentLastVersion,
            Comment = retentionData.SalesArrangement?.Refixation?.Comment
        };

        // zjistit rate ICcka
        decimal? icRate = null;
        if (retentionData.ActivePriceExceptionTaskIdSb.HasValue)
        {
            var taskDetail = await _caseService.GetTaskDetail(retentionData.ActivePriceExceptionTaskIdSb.Value, cancellationToken);
            icRate = taskDetail.TaskDetail.PriceException?.LoanInterestRate?.LoanInterestRateDiscount;
        }

        // seznam nabidek
        response.Offers = (await _mediator.Send(new SimulateMortgageRefixationOfferListRequest
        {
            CaseId = request.CaseId,
            InterestRateDiscount = icRate
        }, cancellationToken))
            .Offers;

        response.ContainsInconsistentIndividualPriceData = !(response.Offers?.All(t => t.InterestRateDiscount == icRate) ?? true);

        return response;
    }
}
