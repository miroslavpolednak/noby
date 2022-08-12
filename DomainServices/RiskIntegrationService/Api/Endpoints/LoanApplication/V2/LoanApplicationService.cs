using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2;

[Authorize]
public class LoanApplicationService
    : _V2.ILoanApplicationService
{
    private readonly IMediator _mediator;

    public LoanApplicationService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask<_V2.LoanApplicationSaveResponse> Save(_V2.LoanApplicationSaveRequest request, CancellationToken cancellationToken = default)
        => await _mediator.Send(request, cancellationToken);
}
