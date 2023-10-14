namespace DomainServices.CustomerService.ExternalServices.Kyc.V1;

internal sealed class MockKycClient
    : IKycClient
{
    public Task SetSocialCharacteristics(long customerId, Contracts.SocialCharacteristics request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.CompletedTask;
    }

    public Task SetKyc(long customerId, Contracts.Kyc request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.CompletedTask;
    }
}
