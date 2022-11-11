using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Clients;
using Contracts = DomainServices.CustomerService.Contracts;

namespace NOBY.Api.Endpoints.Customer.ProfileCheck;

internal sealed class ProfileCheckHandler
    : IRequestHandler<ProfileCheckRequest, ProfileCheckResponse>
{
    private const string ProfileCode = "IDENTIFIED_SUBJECT";

    public async Task<ProfileCheckResponse> Handle(ProfileCheckRequest request, CancellationToken cancellationToken)
    {
        var serviceRequest = new Contracts.ProfileCheckRequest
        {
            Identity = new Identity(request.IdentityId, request.IdentityScheme),
            CustomerProfileCode = ProfileCode
        };

        var result = ServiceCallResult.ResolveAndThrowIfError<Contracts.ProfileCheckResponse>(await _customerService.ProfileCheck(serviceRequest, cancellationToken));

        return new ProfileCheckResponse
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
