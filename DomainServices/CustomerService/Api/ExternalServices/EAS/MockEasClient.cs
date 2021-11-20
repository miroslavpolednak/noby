using DomainServices.CustomerService.Api.ExternalServices.EAS.EasWrapper;

namespace DomainServices.CustomerService.Api.ExternalServices.EAS
{
    internal class MockEasClient : IEasClient
    {
        public Task<int?> GetKlientData(string birthNumber)
        {
            throw new NotImplementedException();
        }

        public Task<S_KLIENTDATA> NewKlient(S_KLIENTDATA client)
        {
            throw new NotImplementedException();
        }
    }
}
