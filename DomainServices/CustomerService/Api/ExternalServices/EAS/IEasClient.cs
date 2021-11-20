using DomainServices.CustomerService.Api.ExternalServices.EAS.EasWrapper;

namespace DomainServices.CustomerService.Api.ExternalServices.EAS;

internal interface IEasClient
{
    public Task<int?> GetKlientData(string birthNumber);

    public Task<S_KLIENTDATA> NewKlient(S_KLIENTDATA client);
}
