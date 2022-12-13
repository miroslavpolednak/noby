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

        var identities = request.Request.Identities;

        return new CreateCustomerResponse
        {
            CreatedCustomerIdentity = identities.Count switch
            {
                1 when IsKbRequest(identities.First()) => await _createIdentifiedSubject.CreateSubject(request.Request, cancellationToken),
                1 or 2 when IsMpRequest(identities) => await _mpDigiClient.CreatePartner(request.Request, cancellationToken),
                _ => throw new InvalidEnumArgumentException()
            }
        };
    }

    private static bool IsKbRequest(Identity identity) => identity.IdentityScheme == Identity.Types.IdentitySchemes.Kb;

    private static bool IsMpRequest(IEnumerable<Identity> identities) =>
        identities.Any(i => i.IdentityId != default && i.IdentityScheme == Identity.Types.IdentitySchemes.Mp);
}