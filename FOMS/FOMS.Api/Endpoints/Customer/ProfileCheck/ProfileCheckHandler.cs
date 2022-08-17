namespace FOMS.Api.Endpoints.Customer.ProfileCheck;

internal sealed class ProfileCheckHandler
    : IRequestHandler<ProfileCheckRequest, ProfileCheckResponse>
{
    public async Task<ProfileCheckResponse> Handle(ProfileCheckRequest request, CancellationToken cancellationToken)
    {
        return new ProfileCheckResponse
        {
            IsCompliant = true
        };
    }

    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public ProfileCheckHandler(DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService)
    {
        _customerService = customerService;
    }
}
