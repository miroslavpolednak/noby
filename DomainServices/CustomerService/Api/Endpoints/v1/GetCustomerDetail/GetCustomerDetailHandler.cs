using System.ComponentModel;
using DomainServices.CustomerService.Api.Services.CustomerManagement;
using ExternalServices.MpHome.V1;
using DomainServices.CustomerService.Api.Services;

namespace DomainServices.CustomerService.Api.Endpoints.v1.GetCustomerDetail;

internal sealed class GetCustomerDetailHandler(
    IMpHomeClient _mpHome,
    MpHomeDetailMapper _mpHomeMapper,
    CustomerManagementDetailProvider _cmDetailProvider)
    : IRequestHandler<CustomerDetailRequest, CustomerDetailResponse>
{
    public Task<CustomerDetailResponse> Handle(CustomerDetailRequest request, CancellationToken cancellationToken)
    {
        return request.Identity.IdentityScheme switch
        {
            SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb => _cmDetailProvider.GetDetail(request.Identity.IdentityId, cancellationToken),
            SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp when request.ForceKbCustomerLoad => loadKBCustomerByMPIdentity(request.Identity.IdentityId, cancellationToken),
            SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp => loadPartner(request.Identity.IdentityId, cancellationToken),
            _ => throw new InvalidEnumArgumentException()
        };
    }

    private async Task<CustomerDetailResponse> loadPartner(long partnerId, CancellationToken cancellationToken)
    {
        var partner = await _mpHome.GetPartner(partnerId, cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CustomerNotFound, partnerId);

        return await _mpHomeMapper.MapDetailResponse(partner, cancellationToken);
    }

    private async Task<CustomerDetailResponse> loadKBCustomerByMPIdentity(long partnerId, CancellationToken cancellationToken)
    {
        var partner = await _mpHome.GetPartner(partnerId, cancellationToken)
            ?? throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.CustomerNotFound, partnerId);

        // primarne chceme detail z CM
        if (!partner.KbId.HasValue)
        {
            return await _mpHomeMapper.MapDetailResponse(partner, cancellationToken);
        }

        var kbDetail = await _cmDetailProvider.GetDetail(partner.KbId!.Value, cancellationToken);
        kbDetail.Identities.Add(new SharedTypes.GrpcTypes.Identity(partnerId, IdentitySchemes.Mp));

        return kbDetail;
    }
}