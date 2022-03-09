using CIS.Core.Results;

namespace DomainServices.RiskIntegrationService.Abstraction;

public interface IRiskIntegrationServiceAbstraction
{
    Task<IServiceCallResult> MyTest(int id, CancellationToken cancellationToken = default(CancellationToken));
}
