using CIS.Infrastructure.Logging;
using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using DomainServices.RiskIntegrationService.Contracts.Shared.V1;

namespace DomainServices.RiskIntegrationService.Abstraction.Services.RiskBusinessCase.V2;

internal class RiskBusinessCaseService
    : Abstraction.RiskBusinessCase.V2.IRiskBusinessCaseServiceAbstraction
{
    public async Task<IServiceCallResult> CreateCase(long salesArrangementId, string? resourceProcessId, CancellationToken cancellationToken = default)
    {
        _logger.RequestHandlerStarted(nameof(CreateCase));
        var result = await _service.CreateCase(new RiskBusinessCaseCreateRequest
        {
            SalesArrangementId = salesArrangementId,
            ResourceProcessId = resourceProcessId
        }, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<RiskBusinessCaseCreateResponse>(result);
    }

    public async Task<IServiceCallResult> CreateAssesment(RiskBusinessCaseCreateAssesmentRequest request, CancellationToken cancellationToken = default)
    {
        _logger.RequestHandlerStarted(nameof(CreateAssesment));
        var result = await _service.CreateAssesment(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<LoanApplicationAssessmentResponse>(result);
    }

    public async Task<IServiceCallResult> GetAssesment(RiskBusinessCaseGetAssesmentRequest request, CancellationToken cancellationToken = default)
    {
        _logger.RequestHandlerStarted(nameof(GetAssesment));
        var result = await _service.GetAssesment(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<LoanApplicationAssessmentResponse>(result);
    }

    public async Task<IServiceCallResult> CommitCase(RiskBusinessCaseCommitCaseRequest request, CancellationToken cancellationToken = default)
    {
        _logger.RequestHandlerStarted(nameof(CommitCase));
        var result = await _service.CommitCase(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<RiskBusinessCaseCommitCaseResponse>(result);
    }

    private readonly ILogger<RiskBusinessCaseService> _logger;
    private readonly IRiskBusinessCaseService _service;

    public RiskBusinessCaseService(IRiskBusinessCaseService service, ILogger<RiskBusinessCaseService> logger)
    {
        _logger = logger;
        _service = service;
    }
}
