using DomainServices.CustomerService.Api.Dto;
using System.ComponentModel;
using DomainServices.CustomerService.Api.Services.CustomerManagement;
using DomainServices.CustomerService.Api.Services.KonsDb;

namespace DomainServices.CustomerService.Api.Handlers;

internal class CreateCustomerHandler : IRequestHandler<CreateCustomerMediatrRequest, CreateCustomerResponse>
{
    private readonly CreateIdentifiedSubject _createIdentifiedSubject;
    private readonly MpDigiCreateClient _mpDigiClient;
    private readonly ILogger<CreateCustomerHandler> _logger;

    public CreateCustomerHandler(CreateIdentifiedSubject createIdentifiedSubject, MpDigiCreateClient mpDigiClient, ILogger<CreateCustomerHandler> logger)
    {
        _createIdentifiedSubject = createIdentifiedSubject;
        _mpDigiClient = mpDigiClient;
        _logger = logger;
    }

    public async Task<CreateCustomerResponse> Handle(CreateCustomerMediatrRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Create customer by request: {request}", request.Request);

        return new CreateCustomerResponse
        {
            CreatedCustomerIdentity = request.Request.Mandant switch
            {
                Mandants.Kb => await _createIdentifiedSubject.CreateSubject(request.Request, cancellationToken),
                Mandants.Mp => await _mpDigiClient.CreatePartner(request.Request, cancellationToken),
                _ => throw new InvalidEnumArgumentException()
            }
        };
    }
}