using CIS.Core.Exceptions;
using ExternalServices.MpHome.V1;
using Google.Protobuf.WellKnownTypes;

namespace DomainServices.CustomerService.Api.Endpoints.UpdateCustomerIdentifiers;

internal sealed class UpdateCustomerIdentifiersHandler 
    : IRequestHandler<UpdateCustomerIdentifiersRequest, Empty>
{
    private readonly IMediator _mediator;
    private readonly IMpHomeClient _mpHomeClient;

    public UpdateCustomerIdentifiersHandler(IMediator mediator, IMpHomeClient mpHomeClient)
    {
        _mediator = mediator;
        _mpHomeClient = mpHomeClient;
    }

    public async Task<Empty> Handle(UpdateCustomerIdentifiersRequest request, CancellationToken cancellationToken)
    {
        switch (request.Mandant)
        {
            case Mandants.Mp:
                await UpdateKbIdInKonsDb(request.CustomerIdentities, cancellationToken);
                break;

            default:
                throw new CisValidationException(11030, "Unsupported mandant");
        }

        return new Empty();
    }

    private async Task UpdateKbIdInKonsDb(ICollection<Identity> identities, CancellationToken cancellationToken)
    {
        var mpIdentity = identities.First(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp);
        var kbIdentity = identities.First(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb);

        //Does partner exist?
        await _mediator.Send(new CustomerDetailRequest { Identity = mpIdentity }, cancellationToken);

        await _mpHomeClient.UpdatePartnerKbId(mpIdentity.IdentityId, kbIdentity.IdentityId, cancellationToken);
    }
}