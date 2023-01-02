using System.ComponentModel;
using DomainServices.CustomerService.Api.Services.CustomerManagement;
using DomainServices.CustomerService.Api.Services.KonsDb;

namespace DomainServices.CustomerService.Api.Endpoints.GetCustomerDetail;

internal class GetCustomerDetailHandler : IRequestHandler<CustomerDetailRequest, CustomerDetailResponse>
{
    private readonly CustomerManagementDetailProvider _cmDetailProvider;
    private readonly KonsDbDetailProvider _konsDbDetailProvider;

    public GetCustomerDetailHandler(CustomerManagementDetailProvider cmDetailProvider, KonsDbDetailProvider konsDbDetailProvider)
    {
        _cmDetailProvider = cmDetailProvider;
        _konsDbDetailProvider = konsDbDetailProvider;
    }

    public Task<CustomerDetailResponse> Handle(CustomerDetailRequest request, CancellationToken cancellationToken)
    {
        return request.Identity.IdentityScheme switch
        {
            Identity.Types.IdentitySchemes.Kb => _cmDetailProvider.GetDetail(request.Identity.IdentityId, cancellationToken),
            Identity.Types.IdentitySchemes.Mp => _konsDbDetailProvider.GetDetail(request.Identity.IdentityId, cancellationToken),
            _ => throw new InvalidEnumArgumentException()
        };
    }
}