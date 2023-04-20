using CIS.Core.Exceptions;
using CIS.Infrastructure.gRPC.CisTypes;
using Microsoft.Extensions.Logging;

namespace NOBY.Infrastructure.Services.CreateOrUpdateCustomerKonsDb;

[CIS.Core.Attributes.TransientService, CIS.Core.Attributes.SelfService]
public sealed class CreateOrUpdateCustomerKonsDbService
{
    public async Task CreateOrUpdate(IEnumerable<Identity> identities, CancellationToken cancellationToken)
    {
        // pokud nema KB identitu, posli ho pryc
        var kbIdentity = identities.FirstOrDefault(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Kb);
        if (kbIdentity is null) return;
        var mpIdentity = identities.FirstOrDefault(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Mp);

        DomainServices.CustomerService.Contracts.CustomerDetailResponse? konsDbCustomer = null;
        try
        {
            //TODO rollbackovat i vytvoreni klienta?
            konsDbCustomer = await _customerService.GetCustomerDetail(mpIdentity!, cancellationToken);
        }
        catch (CisNotFoundException) // klient nenalezen v konsDb
        {
        }

        // klient nenalezen v konsDb, zaloz ho tam
        if (konsDbCustomer is null)
        {
            try
            {
                await createClientInKonsDb(kbIdentity, mpIdentity!, cancellationToken);
            }
            catch
            {
                _logger.LogError("MpDigi createClient failed");
            }
        }
        // ma klient v konsDb KB identitu? pokud ne, tak ho updatuj
        else if (konsDbCustomer.Identities.All(t => t.IdentityScheme != Identity.Types.IdentitySchemes.Kb))
        {
            try
            {
                await updateClientInKonsDb(mpIdentity!, kbIdentity, cancellationToken);
            }
            catch
            {
                _logger.LogError("MpDigi updateClient failed");
            }
        }
    }

    private Task updateClientInKonsDb(Identity mpIdentity, Identity kbIdentity, CancellationToken cancellationToken)
    {
        var request = new DomainServices.CustomerService.Contracts.UpdateCustomerIdentifiersRequest
        {
            Mandant = Mandants.Mp,
            CustomerIdentities = { mpIdentity, kbIdentity }
        };

        return _customerService.UpdateCustomerIdentifiers(request, cancellationToken);
    }

    private async Task createClientInKonsDb(Identity kbIdentity, Identity mpIdentity, CancellationToken cancellationToken)
    {
        // pokud neexistuje customer v konsDb, tak ho vytvor
        var customerDetail = await _customerService.GetCustomerDetail(kbIdentity, cancellationToken);

        // zalozit noveho klienta
        var request = new DomainServices.CustomerService.Contracts.CreateCustomerRequest
        {
            Identities =
            {
                mpIdentity,
                kbIdentity
            },
            HardCreate = true,
            NaturalPerson = customerDetail.NaturalPerson,
            Mandant = Mandants.Mp
        };

        if (customerDetail.IdentificationDocument is not null)
            request.IdentificationDocument = new()
            {
                IssuedBy = customerDetail.IdentificationDocument.IssuedBy,
                IssuedOn = customerDetail.IdentificationDocument.IssuedOn,
                IssuingCountryId = customerDetail.IdentificationDocument.IssuingCountryId,
                Number = customerDetail.IdentificationDocument.Number,
                RegisterPlace = customerDetail.IdentificationDocument.RegisterPlace,
                IdentificationDocumentTypeId = customerDetail.IdentificationDocument.IdentificationDocumentTypeId,
                ValidTo = customerDetail.IdentificationDocument.ValidTo
            };
        if (customerDetail.Addresses is not null && customerDetail.Addresses.Any())
            request.Addresses.Add(customerDetail.Addresses.Where(x => x.AddressTypeId == 1).Select(x => new GrpcAddress
            {
                Street = x.Street,
                City = x.City,
                AddressTypeId = x.AddressTypeId,
                HouseNumber = x.HouseNumber,
                StreetNumber = x.StreetNumber,
                EvidenceNumber = x.EvidenceNumber,
                Postcode = x.Postcode
            }));

        await _customerService.CreateCustomer(request, cancellationToken);
    }

    private readonly DomainServices.CustomerService.Clients.ICustomerServiceClient _customerService;
    private readonly ILogger<CreateOrUpdateCustomerKonsDbService> _logger;

    public CreateOrUpdateCustomerKonsDbService(
        DomainServices.CustomerService.Clients.ICustomerServiceClient customerService,
        ILogger<CreateOrUpdateCustomerKonsDbService> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }
}
