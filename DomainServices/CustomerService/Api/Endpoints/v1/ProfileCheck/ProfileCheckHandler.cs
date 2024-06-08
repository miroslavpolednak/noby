using DomainServices.CustomerService.ExternalServices.CustomerProfile.V1;

namespace DomainServices.CustomerService.Api.Endpoints.v1.ProfileCheck;

internal sealed class ProfileCheckHandler(ICustomerProfileClient _customerProfile)
        : IRequestHandler<ProfileCheckRequest, ProfileCheckResponse>
{
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