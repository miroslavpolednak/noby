using System.ComponentModel;
using DomainServices.CustomerService.Api.Services.CustomerManagement;
using DomainServices.CustomerService.Api.Services.KonsDb;

namespace DomainServices.CustomerService.Api.Endpoints.CreateCustomer;

internal class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest, CreateCustomerResponse>
{
    private readonly CreateIdentifiedSubject _createIdentifiedSubject;
    private readonly MpDigiCreateClient _mpDigiClient;
    
    public CreateCustomerHandler(CreateIdentifiedSubject createIdentifiedSubject, MpDigiCreateClient mpDigiClient)
    {
        _createIdentifiedSubject = createIdentifiedSubject;
        _mpDigiClient = mpDigiClient;
    }

    public async Task<CreateCustomerResponse> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        return new CreateCustomerResponse
        {
            CreatedCustomerIdentity = request.Mandant switch
            {
                Mandants.Kb => await _createIdentifiedSubject.CreateSubject(request, cancellationToken),
                Mandants.Mp => await _mpDigiClient.CreatePartner(request, cancellationToken),
                _ => throw new InvalidEnumArgumentException()
            }
        };
    }
}