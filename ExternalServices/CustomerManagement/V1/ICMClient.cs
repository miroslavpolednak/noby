using ExternalServices.CustomerManagement.V1.CMWrapper;

namespace ExternalServices.CustomerManagement.V1;

public interface ICMClient
{
    Task<IServiceCallResult> Search(SearchCustomerRequest model);
}
