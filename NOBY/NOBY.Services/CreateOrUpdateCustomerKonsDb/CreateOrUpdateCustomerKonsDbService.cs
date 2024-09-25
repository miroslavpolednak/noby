using SharedTypes.GrpcTypes;
using Microsoft.Extensions.Logging;

namespace NOBY.Services.CreateOrUpdateCustomerKonsDb;

[TransientService, SelfService]
public sealed class CreateOrUpdateCustomerKonsDbService(
    DomainServices.CustomerService.Clients.v1.ICustomerServiceClient _customerService,
    ILogger<CreateOrUpdateCustomerKonsDbService> _logger)
{
    public async Task CreateOrUpdate(IEnumerable<Identity> identities, CancellationToken cancellationToken)
    {
        // pokud nema KB identitu, posli ho pryc
        var kbIdentity = identities.GetKbIdentityOrDefault();
        if (kbIdentity is null) return;
        var mpIdentity = identities.GetMpIdentityOrDefault();

        DomainServices.CustomerService.Contracts.Customer? konsDbCustomer = null;
        try
        {
            //TODO rollbackovat i vytvoreni klienta?
            konsDbCustomer = await _customerService.GetCustomerDetail(mpIdentity!, cancellationToken);
        }
        catch (CisNotFoundException) // klient nenalezen v konsDb
        {
            _logger.LogDebug("Client {IdentityId}:{IdentityScheme} not found in KonsDb", mpIdentity?.IdentityId, mpIdentity?.IdentityScheme);
        }

        // klient nenalezen v konsDb, zaloz ho tam
        if (konsDbCustomer is null)
        {
            await createClientInKonsDb(kbIdentity, mpIdentity!, cancellationToken);
        }
        // ma klient v konsDb KB identitu? pokud ne, tak ho updatuj
        else if (!konsDbCustomer.Identities.HasKbIdentity())
        {
            await updateClientInKonsDb(mpIdentity!, kbIdentity, cancellationToken);
        }
    }

    private Task updateClientInKonsDb(Identity mpIdentity, Identity kbIdentity, CancellationToken cancellationToken)
    {
        var request = new DomainServices.CustomerService.Contracts.UpdateCustomerIdentifiersRequest
        {
            Mandant = SharedTypes.GrpcTypes.Mandants.Mp,
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
            Mandant = SharedTypes.GrpcTypes.Mandants.Mp
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
        if (customerDetail.Addresses is not null && customerDetail.Addresses.Count != 0)
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
}
