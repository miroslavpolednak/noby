using CIS.Core.Results;
using CIS.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace DomainServices.RiskIntegrationService.Abstraction.Services;

internal class RiskIntegrationService : IRiskIntegrationServiceAbstraction
{
    public async Task<IServiceCallResult> MyTest(int id, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.RequestHandlerStartedWithId(nameof(RiskIntegrationService), id);
        var result = await _userContext.AddUserContext(async () => await _service.MyTestAsync(new Contracts.MyTestRequest() { Id = id }, cancellationToken: cancellationToken));
        return new SuccessfulServiceCallResult<Contracts.MyTestResponse>(result);
    }

    private readonly ILogger<RiskIntegrationService> _logger;
    private readonly Contracts.v1.RiskIntegrationService.RiskIntegrationServiceClient _service;
    private readonly CIS.Security.InternalServices.ICisUserContextHelpers _userContext;

    public RiskIntegrationService(
        ILogger<RiskIntegrationService> logger,
        Contracts.v1.RiskIntegrationService.RiskIntegrationServiceClient service,
        CIS.Security.InternalServices.ICisUserContextHelpers userContext)
    {
        _userContext = userContext;
        _service = service;
        _logger = logger;
    }
}
