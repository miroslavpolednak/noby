using System.ComponentModel;
using DomainServices.CustomerService.Api.Services.CustomerManagement;
using DomainServices.CustomerService.Api.Services.KonsDb;

namespace DomainServices.CustomerService.Api.Endpoints.UpdateCustomer;

internal sealed class UpdateCustomerHandler 
    : IRequestHandler<UpdateCustomerRequest, UpdateCustomerResponse>
{
    private readonly IdentifiedSubjectService _identifiedSubjectService;
    private readonly MpDigiClient _mpDigiClient;

    public UpdateCustomerHandler(
        IdentifiedSubjectService identifiedSubjectService,
        MpDigiClient mpDigiClient)
    {
        _identifiedSubjectService = identifiedSubjectService;
        _mpDigiClient = mpDigiClient;
    }
    
    public async Task<UpdateCustomerResponse> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        if (request.Mandant == Mandants.Kb)
        {
            await _identifiedSubjectService.UpdateSubject(request, cancellationToken);
        }
        else if (request.Mandant == Mandants.Mp)
        {
            await _mpDigiClient.UpdatePartner(request, cancellationToken);
        }
        else
        {
            throw new InvalidEnumArgumentException();
        }
        
        return new UpdateCustomerResponse();
    }
}