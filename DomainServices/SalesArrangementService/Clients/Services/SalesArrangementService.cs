using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Clients.Services;

internal sealed class SalesArrangementService 
    : ISalesArrangementServiceClient
{
    public async Task<(int SalesArrangementId, int? OfferId)> GetProductSalesArrangement(long caseId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var response = (await _service.GetProductSalesArrangementAsync(
            new()
            {
                CaseId = caseId,
            }, cancellationToken: cancellationToken)
            );
        return (response.SalesArrangementId, response.OfferId);
    }

    public async Task DeleteSalesArrangement(int salesArrangementId, bool hardDelete = false, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.DeleteSalesArrangementAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                HardDelete = hardDelete
            }, cancellationToken: cancellationToken);
    }

    public async Task<int> CreateSalesArrangement(long caseId, int salesArrangementTypeId, int? offerId = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.CreateSalesArrangementAsync(
            new() { 
                CaseId = caseId, 
                SalesArrangementTypeId = salesArrangementTypeId, 
                OfferId = offerId
            }, cancellationToken: cancellationToken);
        return result.SalesArrangementId;
    }

    public async Task<int> CreateSalesArrangement(CreateSalesArrangementRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.CreateSalesArrangementAsync(request, cancellationToken: cancellationToken);
        return result.SalesArrangementId;
    }

    public async Task<SalesArrangement> GetSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.GetSalesArrangementAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken);
    }
    
    public async Task<SalesArrangement?> GetSalesArrangementByOfferId(int offerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetSalesArrangementByOfferIdAsync(
            new()
            {
                OfferId = offerId
            }, cancellationToken: cancellationToken);
        return result.IsExisting ? result.Instance : null;
    }

    public async Task LinkModelationToSalesArrangement(int salesArrangementId, int offerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.LinkModelationToSalesArrangementAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                OfferId = offerId
            }, cancellationToken: cancellationToken);
    }

    public async Task<GetSalesArrangementListResponse> GetSalesArrangementList(long caseId, CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _service.GetSalesArrangementListAsync(
            new()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken);
    }

    public async Task UpdateSalesArrangement(int salesArrangementId, string? contractNumber, string? riskBusinessCaseId, CancellationToken cancellationToken = default(CancellationToken))
    {
         await _service.UpdateSalesArrangementAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                ContractNumber = contractNumber ?? "",
                RiskBusinessCaseId = riskBusinessCaseId ?? ""
            }, cancellationToken: cancellationToken);
    }

    public async Task UpdateSalesArrangementState(int salesArrangementId, int state, CancellationToken cancellationToken = default(CancellationToken))
    {
         await _service.UpdateSalesArrangementStateAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                State = state
            }, cancellationToken: cancellationToken);
    }

    public async Task UpdateSalesArrangementParameters(Contracts.UpdateSalesArrangementParametersRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
         await _service.UpdateSalesArrangementParametersAsync(request, cancellationToken: cancellationToken);
    }

    public async Task<ValidateSalesArrangementResponse> ValidateSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default)
    {
         return await _service.ValidateSalesArrangementAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken);
    }

    public async Task SendToCmp(int salesArrangementId, CancellationToken cancellationToken = default)
    {
         await _service.SendToCmpAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken);
    }

    public async Task UpdateLoanAssessmentParameters(int salesArrangementId, string? loanApplicationAssessmentId, string? riskSegment, string? commandId, DateTime? riskBusinessCaseExpirationDate, CancellationToken cancellationToken = default(CancellationToken))
    {
         await _service.UpdateLoanAssessmentParametersAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                LoanApplicationAssessmentId = loanApplicationAssessmentId ?? "",
                RiskSegment = riskSegment ?? "",
                CommandId = commandId ?? "",
                RiskBusinessCaseExpirationDate = riskBusinessCaseExpirationDate,
            }, cancellationToken: cancellationToken);
    }

    public async Task UpdateOfferDocumentId(int salesArrangementId, string offerDocumentId, CancellationToken cancellationToken = default(CancellationToken))
    {
        await _service.UpdateOfferDocumentIdAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                OfferDocumentId = offerDocumentId
            }, cancellationToken: cancellationToken);
    }

    public async Task<List<FlowSwitch>> GetFlowSwitches(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        return (await _service.GetFlowSwitchesAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken)).FlowSwitches.ToList();
    }

    public async Task SetFlowSwitches(int salesArrangementId, List<FlowSwitch> flowSwitches, CancellationToken cancellationToken = default(CancellationToken))
    {
        var request = new SetFlowSwitchesRequest
        {
            SalesArrangementId = salesArrangementId
        };
        request.FlowSwitches.AddRange(flowSwitches);

        await _service.SetFlowSwitchesAsync(request, cancellationToken: cancellationToken);
    }

    private readonly Contracts.v1.SalesArrangementService.SalesArrangementServiceClient _service;

    public SalesArrangementService(Contracts.v1.SalesArrangementService.SalesArrangementServiceClient service)
    {
        _service = service;
    }
}
