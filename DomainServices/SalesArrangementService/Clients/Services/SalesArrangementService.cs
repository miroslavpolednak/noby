using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Clients.Services;

internal class SalesArrangementService : ISalesArrangementServiceClients
{
    public async Task<IServiceCallResult> DeleteSalesArrangement(int salesArrangementId, bool hardDelete = false, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.DeleteSalesArrangementAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                HardDelete = hardDelete
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> CreateSalesArrangement(long caseId, int salesArrangementTypeId, int? offerId = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.CreateSalesArrangementAsync(
            new() { 
                CaseId = caseId, 
                SalesArrangementTypeId = salesArrangementTypeId, 
                OfferId = offerId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<int>(result.SalesArrangementId);
    }

    public async Task<IServiceCallResult> CreateSalesArrangement(CreateSalesArrangementRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.CreateSalesArrangementAsync(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<int>(result.SalesArrangementId);
    }

    public async Task<IServiceCallResult> GetSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetSalesArrangementAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<SalesArrangement>(result);
    }
    
    public async Task<IServiceCallResult> GetSalesArrangementByOfferId(int offerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetSalesArrangementByOfferIdAsync(
            new()
            {
                OfferId = offerId
            }, cancellationToken: cancellationToken);
        return result.IsExisting ? new SuccessfulServiceCallResult<SalesArrangement>(result.Instance) : new EmptyServiceCallResult();
    }

    public async Task<IServiceCallResult> LinkModelationToSalesArrangement(int salesArrangementId, int offerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.LinkModelationToSalesArrangementAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                OfferId = offerId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> GetSalesArrangementList(long caseId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.GetSalesArrangementListAsync(
            new()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<GetSalesArrangementListResponse>(result);
    }

    public async Task<IServiceCallResult> UpdateSalesArrangement(int salesArrangementId, string? contractNumber, string? riskBusinessCaseId, DateTime? firstSignedDate, CancellationToken cancellationToken = default(CancellationToken))
    {
         var result = await _service.UpdateSalesArrangementAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                ContractNumber = contractNumber ?? "",
                RiskBusinessCaseId = riskBusinessCaseId ?? "",
                FirstSignedDate = firstSignedDate,
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateSalesArrangementState(int salesArrangementId, int state, CancellationToken cancellationToken = default(CancellationToken))
    {
         var result = await _service.UpdateSalesArrangementStateAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                State = state
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateSalesArrangementParameters(Contracts.UpdateSalesArrangementParametersRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
         var result = await _service.UpdateSalesArrangementParametersAsync(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> ValidateSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default)
    {
         var result = await _service.ValidateSalesArrangementAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<ValidateSalesArrangementResponse>(result);
    }

    public async Task<IServiceCallResult> SendToCmp(int salesArrangementId, CancellationToken cancellationToken = default)
    {
         var result = await _service.SendToCmpAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateLoanAssessmentParameters(int salesArrangementId, string? loanApplicationAssessmentId, string? riskSegment, string? commandId, DateTime? riskBusinessCaseExpirationDate, CancellationToken cancellationToken = default(CancellationToken))
    {
         var result = await _service.UpdateLoanAssessmentParametersAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                LoanApplicationAssessmentId = loanApplicationAssessmentId ?? "",
                RiskSegment = riskSegment ?? "",
                CommandId = commandId ?? "",
                RiskBusinessCaseExpirationDate = riskBusinessCaseExpirationDate,
            }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult();
    }

    private readonly ILogger<SalesArrangementService> _logger;
    private readonly Contracts.v1.SalesArrangementService.SalesArrangementServiceClient _service;

    public SalesArrangementService(
        ILogger<SalesArrangementService> logger,
        Contracts.v1.SalesArrangementService.SalesArrangementServiceClient service)
    {
        _service = service;
        _logger = logger;
    }
}
