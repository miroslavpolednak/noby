using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.RiskIntegrationService.ExternalServices.CustomersExposure.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CustomersExposure.V1;

internal interface ICustomersExposureClient
    : IExternalServiceClient
{
    Task<LoanApplicationRelatedExposureResult> Calculate(LoanApplicationRelatedExposure request, CancellationToken cancellationToken);

    const string Version = "V1";
}