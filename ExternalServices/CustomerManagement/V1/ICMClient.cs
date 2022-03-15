using ExternalServices.CustomerManagement.V1.CMWrapper;

namespace ExternalServices.CustomerManagement.V1;

public interface ICMClient
{
    Task<IServiceCallResult> Search(SearchCustomerRequest model, CancellationToken cancellationToken = default);

    Task<IServiceCallResult> GetList(IEnumerable<long> model, CancellationToken cancellationToken = default);

    Task<IServiceCallResult> GetDetail(long model, CancellationToken cancellationToken = default);
}
