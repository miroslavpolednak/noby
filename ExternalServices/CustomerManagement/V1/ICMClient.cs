using ExternalServices.CustomerManagement.V1.CMWrapper;

namespace ExternalServices.CustomerManagement.V1;

public interface ICMClient
{
    Task<IServiceCallResult> Search(SearchCustomerRequest model, string TraceId, CancellationToken cancellationToken = default);

    Task<IServiceCallResult> GetList(IEnumerable<long> model, string TraceId, CancellationToken cancellationToken = default);

    Task<IServiceCallResult> GetDetail(long model, string TraceId, CancellationToken cancellationToken = default);
}
