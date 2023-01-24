using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.CustomerService.ExternalServices.Kyc.V1;

public interface IKycClient
    : IExternalServiceClient
{
    Task SetSocialCharacteristics(int kbCustomerId, Contracts.SocialCharacteristics request, CancellationToken cancellationToken = default(CancellationToken));

    Task SetKyc(int kbCustomerId, Contracts.Kyc request, CancellationToken cancellationToken = default(CancellationToken));

    Task SetFinancialProfile(int kbCustomerId, Contracts.EmploymentFinancialProfile request, CancellationToken cancellationToken = default(CancellationToken));

    const string Version = "V1";
}
