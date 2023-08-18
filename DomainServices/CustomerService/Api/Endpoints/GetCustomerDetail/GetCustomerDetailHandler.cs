using System.ComponentModel;
using CIS.Foms.Enums;
using DomainServices.CustomerService.Api.Services.CustomerManagement;
using DomainServices.CustomerService.Api.Services.KonsDb;

namespace DomainServices.CustomerService.Api.Endpoints.GetCustomerDetail;

internal sealed class GetCustomerDetailHandler : IRequestHandler<CustomerDetailRequest, CustomerDetailResponse>
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
            Identity.Types.IdentitySchemes.Mp when request.ForceKbCustomerLoad => LoadKBCustomerByMPIdentity(request.Identity.IdentityId, cancellationToken),
            Identity.Types.IdentitySchemes.Mp => _konsDbDetailProvider.GetDetail(request.Identity.IdentityId, cancellationToken),
            _ => throw new InvalidEnumArgumentException()
        };
    }

    private async Task<CustomerDetailResponse> LoadKBCustomerByMPIdentity(long partnerId, CancellationToken cancellationToken)
    {
        var partner = await _konsDbDetailProvider.GetDetail(partnerId, cancellationToken);

        var kbIdentity = partner.Identities.FirstOrDefault(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb);

        if (kbIdentity is null)
            return partner;

        var kbDetail = await _cmDetailProvider.GetDetail(kbIdentity.IdentityId, cancellationToken);

        kbDetail.Identities.Add(new Identity(partnerId, IdentitySchemes.Mp));

        return kbDetail;
    }
}