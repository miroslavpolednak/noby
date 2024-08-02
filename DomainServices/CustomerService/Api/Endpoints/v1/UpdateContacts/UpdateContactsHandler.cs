using DomainServices.CustomerService.Api.Services.CustomerManagement;
using SharedTypes.GrpcTypes;

namespace DomainServices.CustomerService.Api.Endpoints.v1.UpdateContacts;

public class UpdateContactsHandler(CustomerManagementContactService contactService) : IRequestHandler<UpdateContactsRequest>
{
    public async Task Handle(UpdateContactsRequest request, CancellationToken cancellationToken)
    {
        if (request.Identity.IdentityScheme != Identity.Types.IdentitySchemes.Kb)
            return;

        await contactService.UpdateEmail(request.Identity.IdentityId, request.Contacts, cancellationToken);
    }
}