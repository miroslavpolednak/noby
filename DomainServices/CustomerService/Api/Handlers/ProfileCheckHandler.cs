using DomainServices.CustomerService.Api.Clients.CustomerProfile.V1;
using DomainServices.CustomerService.Api.Dto;
using DomainServices.CustomerService.Contracts;
using System.Diagnostics;
using DomainServices.CustomerService.Api.Extensions;

namespace DomainServices.CustomerService.Api.Handlers;

internal class ProfileCheckHandler : IRequestHandler<ProfileCheckMediatrRequest, ProfileCheckResponse>
{
    private readonly ICustomerProfileClient _customerProfile;

    public ProfileCheckHandler(ICustomerProfileClient customerProfile)
    {
        _customerProfile = customerProfile;
    }

    public async Task<ProfileCheckResponse> Handle(ProfileCheckMediatrRequest request, CancellationToken cancellationToken)
    {
        var input = request.Request;

        var result = await _customerProfile.ValidateProfile(input.Identity.IdentityId,
                                                            input.CustomerProfileCode,
                                                            Activity.Current?.TraceId.ToHexString() ?? "",
                                                            cancellationToken)
                                           .CheckCustomerProfileResult<bool>();

        return new ProfileCheckResponse
        {
            IsCompliant = result
        };
    }
}