using CIS.Infrastructure.Logging;
using DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

namespace DomainServices.RiskIntegrationService.Abstraction.Services.LoanApplication.V2;

internal class LoanApplicationService
    : Abstraction.LoanApplication.V2.ILoanApplicationServiceAbstraction
{
    public async Task<IServiceCallResult> Save(LoanApplicationSaveRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStarted(nameof(Save));
        var result = await _service.Save(request, cancellationToken: cancellationToken);
        return new SuccessfulServiceCallResult<LoanApplicationSaveResponse>(result);
    }

    private readonly ILogger<LoanApplicationService> _logger;
    private readonly ILoanApplicationService _service;

    public LoanApplicationService(ILoanApplicationService service, ILogger<LoanApplicationService> logger)
    {
        _logger = logger;
        _service = service;
    }
}
