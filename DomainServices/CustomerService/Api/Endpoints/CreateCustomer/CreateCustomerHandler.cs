using System.ComponentModel;
using DomainServices.CustomerService.Api.Services.CustomerManagement;
using DomainServices.CustomerService.Api.Services.KonsDb;

namespace DomainServices.CustomerService.Api.Endpoints.CreateCustomer;

internal class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest, CreateCustomerResponse>
{
    private readonly IdentifiedSubjectService _identifiedSubjectService;
    private readonly MpDigiClient _mpDigiClient;
    
    public CreateCustomerHandler(IdentifiedSubjectService identifiedSubjectService, MpDigiClient mpDigiClient)
    {
        _identifiedSubjectService = identifiedSubjectService;
        _mpDigiClient = mpDigiClient;
    }

    public async Task<CreateCustomerResponse> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        return new CreateCustomerResponse
        {
            CreatedCustomerIdentity = request.Mandant switch
            {
                Mandants.Kb => await _identifiedSubjectService.CreateSubject(request, cancellationToken),
                Mandants.Mp => await _mpDigiClient.CreatePartner(request, cancellationToken),
                _ => throw new InvalidEnumArgumentException()
            }
        };
    }
}