using DomainServices.CustomerService.Clients;
using Contracts = DomainServices.CustomerService.Contracts;

namespace NOBY.Api.Endpoints.Customer.ProfileCheck;

internal sealed class ProfileCheckHandler
    : IRequestHandler<ProfileCheckRequest, CustomerProfileCheckResponse>
{
    private const string ProfileCode = "IDENTIFIED_SUBJECT";

    public async Task<CustomerProfileCheckResponse> Handle(ProfileCheckRequest request, CancellationToken cancellationToken)
    {
        var serviceRequest = new Contracts.ProfileCheckRequest
        {
            Identity = request.Identity,
            CustomerProfileCode = ProfileCode
        };

        var result = await _customerService.ProfileCheck(serviceRequest, cancellationToken);

        return new CustomerProfileCheckResponse
        {
            IsCompliant = result.IsCompliant
        };
    }

    private readonly ICustomerServiceClient _customerService;

    public ProfileCheckHandler(ICustomerServiceClient customerService)
    {
        _customerService = customerService;
    }
}
