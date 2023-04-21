using DomainServices.CustomerService.ExternalServices.CustomerProfile.V1;

namespace DomainServices.CustomerService.Api.Endpoints.ProfileCheck;

internal sealed class ProfileCheckHandler : IRequestHandler<ProfileCheckRequest, ProfileCheckResponse>
{
    private readonly ICustomerProfileClient _customerProfile;

    public ProfileCheckHandler(ICustomerProfileClient customerProfile)
    {
        _customerProfile = customerProfile;
    }

    public async Task<ProfileCheckResponse> Handle(ProfileCheckRequest request, CancellationToken cancellationToken)
    {
        var result = await _customerProfile.ValidateProfile(request.Identity.IdentityId,
                                                            request.CustomerProfileCode,
                                                            cancellationToken);

        return new ProfileCheckResponse
        {
            IsCompliant = result
        };
    }
}