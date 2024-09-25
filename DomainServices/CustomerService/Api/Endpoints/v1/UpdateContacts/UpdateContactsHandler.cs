using DomainServices.CustomerService.Api.Services.CustomerManagement;
using Google.Protobuf.WellKnownTypes;
using SharedTypes.GrpcTypes;

namespace DomainServices.CustomerService.Api.Endpoints.v1.UpdateContacts;

public class UpdateContactsHandler(CustomerManagementContactService contactService) : IRequestHandler<UpdateContactsRequest, Empty>
{
    public async Task<Empty> Handle(UpdateContactsRequest request, CancellationToken cancellationToken)
    {
        if (request.Identity.IdentityScheme == Identity.Types.IdentitySchemes.Kb)
            await contactService.UpdateEmail(request.Identity.IdentityId, request.Contacts, cancellationToken);

        return new();
    }
}