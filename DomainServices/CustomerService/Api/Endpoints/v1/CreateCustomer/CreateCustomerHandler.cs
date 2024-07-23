using System.ComponentModel;
using DomainServices.CustomerService.Api.Services.CustomerManagement;
using ExternalServices.MpHome.V1;

namespace DomainServices.CustomerService.Api.Endpoints.v1.CreateCustomer;

internal sealed class CreateCustomerHandler(
    IdentifiedSubjectService _identifiedSubjectService,
    Services.MpHomeUpdateMapper _mpHomeMapper,
    IMpHomeClient _mpHomeClient)
    : IRequestHandler<CreateCustomerRequest, CreateCustomerResponse>
{
    public async Task<CreateCustomerResponse> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        return request.Mandant switch
        {
            SharedTypes.GrpcTypes.Mandants.Kb => await _identifiedSubjectService.CreateSubject(request, cancellationToken),
            SharedTypes.GrpcTypes.Mandants.Mp => await createPartner(request, cancellationToken),
            _ => throw new InvalidEnumArgumentException()
        };
    }

    private async Task<CreateCustomerResponse> createPartner(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var mpIdentity = request.Identities.First(i => i.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp);

        if (await _mpHomeClient.PartnerExists(mpIdentity.IdentityId, cancellationToken))
        {
            //throw new CisAlreadyExistsException(11017, "Partner already exists in KonsDB.");
        }

        var partnerRequest = await _mpHomeMapper.MapUpdateRequest(request.NaturalPerson, request.Identities, request.IdentificationDocument, request.Addresses, request.Contacts, cancellationToken);

        await _mpHomeClient.UpdatePartner(mpIdentity.IdentityId, partnerRequest, cancellationToken);

        return new CreateCustomerResponse
        {
            CreatedCustomerIdentity = mpIdentity
        };
    }
}