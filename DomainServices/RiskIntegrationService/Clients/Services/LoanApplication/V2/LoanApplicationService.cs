using DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

namespace DomainServices.RiskIntegrationService.Clients.Services.LoanApplication.V2;

internal sealed class LoanApplicationService
    : Clients.LoanApplication.V2.ILoanApplicationServiceClient
{
    public async Task<string> Save(LoanApplicationSaveRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        var result = await _service.Save(request, cancellationToken: cancellationToken);
        return result.RiskSegment.ToString();
    }

    private readonly ILoanApplicationService _service;

    public LoanApplicationService(ILoanApplicationService service)
    {
        _service = service;
    }
}
