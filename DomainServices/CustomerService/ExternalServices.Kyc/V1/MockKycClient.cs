namespace DomainServices.CustomerService.ExternalServices.Kyc.V1;

internal sealed class MockKycClient
    : IKycClient
{
    public Task SetSocialCharacteristics(int kbCustomerId, Contracts.SocialCharacteristics request, CancellationToken cancellationToken = default(CancellationToken))
    {
        return Task.CompletedTask;
    }
}
