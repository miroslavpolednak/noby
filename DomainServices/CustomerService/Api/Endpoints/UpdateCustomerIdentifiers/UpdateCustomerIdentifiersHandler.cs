using CIS.Core.Exceptions;
using ExternalServices.MpHome.V1_1;

namespace DomainServices.CustomerService.Api.Endpoints.UpdateCustomerIdentifiers;

internal class UpdateCustomerIdentifiersHandler : IRequestHandler<UpdateCustomerIdentifiersRequest>
{
    private readonly IMediator _mediator;
    private readonly IMpHomeClient _mpHomeClient;

    public UpdateCustomerIdentifiersHandler(IMediator mediator, IMpHomeClient mpHomeClient)
    {
        _mediator = mediator;
        _mpHomeClient = mpHomeClient;
    }

    public async Task<Unit> Handle(UpdateCustomerIdentifiersRequest request, CancellationToken cancellationToken)
    {
        switch (request.Mandant)
        {
            case Mandants.Mp:
                await UpdateKbIdInKonsDb(request.CustomerIdentities, cancellationToken);
                break;

            default:
                throw new CisValidationException(11030, "Unsupported mandant");
        }

        return Unit.Value;
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