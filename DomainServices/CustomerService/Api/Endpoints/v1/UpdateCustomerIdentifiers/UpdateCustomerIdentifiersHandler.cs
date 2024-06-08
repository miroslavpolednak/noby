using CIS.Core.Exceptions;
using ExternalServices.MpHome.V1;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.CustomerService.Api.Endpoints.v1.UpdateCustomerIdentifiers;

internal sealed class UpdateCustomerIdentifiersHandler(
    IMediator _mediator,
    IMpHomeClient _mpHomeClient)
        : IRequestHandler<UpdateCustomerIdentifiersRequest, Empty>
{
    public async Task<Empty> Handle(UpdateCustomerIdentifiersRequest request, CancellationToken cancellationToken)
    {
        switch (request.Mandant)
        {
            case SharedTypes.GrpcTypes.Mandants.Mp:
                await updateKbIdInKonsDb(request.CustomerIdentities, cancellationToken);
                break;

            default:
                throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.UnsupportedMandant);
        }

        return new Empty();
    }

    private async Task updateKbIdInKonsDb(ICollection<SharedTypes.GrpcTypes.Identity> identities, CancellationToken cancellationToken)
    {
        var mpIdentity = identities.First(i => i.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp);
        var kbIdentity = identities.First(i => i.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb);

        //Does partner exist?
        try
        {
            await _mediator.Send(new CustomerDetailRequest { Identity = mpIdentity }, cancellationToken);
        }
        catch (CisNotFoundException)
        {
            throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(ErrorCodeMapper.IdentityNotExist, mpIdentity.IdentityId);
        }

        await _mpHomeClient.UpdatePartnerKbId(mpIdentity.IdentityId, kbIdentity.IdentityId, cancellationToken);
    }
}