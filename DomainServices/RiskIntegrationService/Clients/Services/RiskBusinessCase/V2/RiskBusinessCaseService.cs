using CIS.Infrastructure.Logging;
using DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using DomainServices.RiskIntegrationService.Contracts.Shared.V1;

namespace DomainServices.RiskIntegrationService.Clients.Services.RiskBusinessCase.V2;

internal class RiskBusinessCaseService
    : Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient
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

    public async Task<IServiceCallResult> CreateAssessment(RiskBusinessCaseCreateAssessmentRequest request, CancellationToken cancellationToken = default)
    {
        _logger.RequestHandlerStarted(nameof(CreateAssessment));
        var result = await _service.CreateAssessment(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<LoanApplicationAssessmentResponse>(result);
    }

    public async Task<IServiceCallResult> GetAssessment(RiskBusinessCaseGetAssessmentRequest request, CancellationToken cancellationToken = default)
    {
        _logger.RequestHandlerStarted(nameof(GetAssessment));
        var result = await _service.GetAssessment(request, cancellationToken: cancellationToken);
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
