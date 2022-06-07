using CIS.DomainServicesSecurity.Abstraction;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Abstraction.Services;

internal class SalesArrangementService : ISalesArrangementServiceAbstraction
{
    public async Task<IServiceCallResult> CreateSalesArrangement(long caseId, int salesArrangementTypeId, int? offerId = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateSalesArrangement), caseId);
        var result = await _userContext.AddUserContext(async () => await _service.CreateSalesArrangementAsync(
            new() { 
                CaseId = caseId, 
                SalesArrangementTypeId = salesArrangementTypeId, 
                OfferId = offerId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<int>(result.SalesArrangementId);
    }

    public async Task<IServiceCallResult> GetSalesArrangement(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetSalesArrangement), salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.GetSalesArrangementAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<SalesArrangement>(result);
    }
    
    public async Task<IServiceCallResult> GetSalesArrangementByOfferId(int offerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetSalesArrangementByOfferId), offerId);
        var result = await _userContext.AddUserContext(async () => await _service.GetSalesArrangementByOfferIdAsync(
            new()
            {
                OfferId = offerId
            }, cancellationToken: cancellationToken)
        );
        return result.IsExisting ? new SuccessfulServiceCallResult<SalesArrangement>(result.Instance) : new EmptyServiceCallResult();
    }

    public async Task<IServiceCallResult> LinkModelationToSalesArrangement(int salesArrangementId, int offerId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(LinkModelationToSalesArrangement), salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.LinkModelationToSalesArrangementAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                OfferId = offerId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> GetSalesArrangementList(long caseId, IEnumerable<int>? states, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(GetSalesArrangementList), caseId);
        var result = await _userContext.AddUserContext(async () => await _service.GetSalesArrangementListAsync(
            new()
            {
                CaseId = caseId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult<GetSalesArrangementListResponse>(result);
    }

    public async Task<IServiceCallResult> UpdateSalesArrangement(int salesArrangementId, string? contractNumber, string? riskBusinessCaseId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateSalesArrangement), salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.UpdateSalesArrangementAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                ContractNumber = contractNumber ?? "",
                RiskBusinessCaseId = riskBusinessCaseId ?? ""
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateSalesArrangementState(int salesArrangementId, int state, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateSalesArrangementState), salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.UpdateSalesArrangementStateAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                State = state
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateSalesArrangementParameters(Contracts.UpdateSalesArrangementParametersRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateSalesArrangementParameters), request.SalesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.UpdateSalesArrangementParametersAsync(request, cancellationToken: cancellationToken));
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> SendToCmp(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        _logger.RequestHandlerStartedWithId(nameof(SendToCmp), salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.SendToCmpAsync(
            new()
            {
                SalesArrangementId = salesArrangementId
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    public async Task<IServiceCallResult> UpdateLoanAssessmentParameters(int salesArrangementId, int? loanApplicationAssessmentId, string? riskSegment, string? commandId, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(UpdateLoanAssessmentParameters), salesArrangementId);
        var result = await _userContext.AddUserContext(async () => await _service.UpdateLoanAssessmentParametersAsync(
            new()
            {
                SalesArrangementId = salesArrangementId,
                LoanApplicationAssessmentId = loanApplicationAssessmentId,
                RiskSegment = riskSegment ?? "",
                CommandId = commandId ?? ""
            }, cancellationToken: cancellationToken)
        );
        return new SuccessfulServiceCallResult();
    }

    private readonly ILogger<SalesArrangementService> _logger;
    private readonly Contracts.v1.SalesArrangementService.SalesArrangementServiceClient _service;
    private readonly ICisUserContextHelpers _userContext;

    public SalesArrangementService(
        ILogger<SalesArrangementService> logger,
        Contracts.v1.SalesArrangementService.SalesArrangementServiceClient service,
        ICisUserContextHelpers userContext)
    {
        _userContext = userContext;
        _service = service;
        _logger = logger;
    }
}
