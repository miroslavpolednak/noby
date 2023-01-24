namespace DomainServices.CustomerService.ExternalServices.Kyc.V1;

internal sealed class MockKycClient
    : IKycClient
{
    public Task SetSocialCharacteristics(int kbCustomerId, Contracts.SocialCharacteristics request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.CompletedTask;
    }

    public Task SetKyc(int kbCustomerId, Contracts.Kyc request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.CompletedTask;
    }

    public Task SetFinancialProfile(int kbCustomerId, Contracts.EmploymentFinancialProfile request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.CompletedTask;
    }
}
