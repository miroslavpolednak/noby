using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Contracts;
using ProtoBuf.Serializers;

namespace NOBY.Api.Endpoints.SalesArrangement.GetCustomers;

internal class GetCustomersHandler
    : IRequestHandler<GetCustomersRequest, List<Dto.CustomerListItem>>
{
    //TODO tohle se bude nejspis cele predelavat, nema smysl to moc resit
    public async Task<List<Dto.CustomerListItem>> Handle(GetCustomersRequest request, CancellationToken cancellationToken)
    {
        // najit existujici customeryOnSA
        var customersOnSA = ServiceCallResult.ResolveAndThrowIfError<List<CustomerOnSA>>(await _customerOnSaService.GetCustomerList(request.SalesArrangementId, cancellationToken));

        _logger.FoundItems(customersOnSA.Count, nameof(CustomerOnSA));

        List<Dto.CustomerListItem> model = new();
        
        //TODO idealne natahnout z customerService vsechny najednou?
        foreach (var t in customersOnSA)
        {
            var c = new Dto.CustomerListItem()
            {
                Id = t.CustomerOnSAId,
                FirstName = t.FirstNameNaturalPerson,
                LastName = t.Name,
                DateOfBirth = t.DateOfBirthNaturalPerson,
                CustomerRoleId = t.CustomerRoleId,
                MaritalStatusId = t.MaritalStatusId
            };
            
            // pokud nema identitu, ani nevolej customerSvc
            if (t.CustomerIdentifiers is not null && t.CustomerIdentifiers.Any())
            {
                c.Identities = t.CustomerIdentifiers.Select(x => new CIS.Foms.Types.CustomerIdentity(x.IdentityId, (int)x.IdentityScheme)).ToList();

                // zavolat customer svc pro detail
                //TODO nejak prioritizovat schemata?
                var identity = new CIS.Infrastructure.gRPC.CisTypes.Identity
                {
                    IdentityId = t.CustomerIdentifiers[0].IdentityId,
                    IdentityScheme = t.CustomerIdentifiers[0].IdentityScheme
                };
                var customerDetail = ServiceCallResult.ResolveAndThrowIfError<CustomerDetailResponse>(await _customerService.GetCustomerDetail(identity, cancellationToken));

                // doplnit detail customera
                c.BirthNumber = customerDetail.NaturalPerson.BirthNumber;
                c.PlaceOfBirth = customerDetail.NaturalPerson.PlaceOfBirth;

                // adresa
                //TODO kterou adresu brat?
                var address = customerDetail.Addresses?.FirstOrDefault(t => t.AddressTypeId == 1);
                if (address is not null)
                {
                    c.MainAddress = new CIS.Foms.Types.Address
                    {
                        Street = address.Street,
                        City = address.City,
                        StreetNumber = address.StreetNumber,
                        CountryId = address.CountryId,
                        HouseNumber = address.HouseNumber,
                        Postcode = address.Postcode
                    };
                }
                // kontaktni adresa
                var address2 = customerDetail.Addresses?.FirstOrDefault(t => t.AddressTypeId == 2);
                if (address2 is not null)
                {
                    c.ContactAddress = new CIS.Foms.Types.Address
                    {
                        Street = address2.Street,
                        City = address2.City,
                        StreetNumber = address2.StreetNumber,
                        CountryId = address2.CountryId,
                        HouseNumber = address2.HouseNumber,
                        Postcode = address2.Postcode
                    };
                }

                // kontakty
                //TODO jak poznam jake kontakty se maji naplnit?
                c.Phone = customerDetail.Contacts?.FirstOrDefault(x => x.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.Mobil)?.Value;
                c.Email = customerDetail.Contacts?.FirstOrDefault(x => x.ContactTypeId == (int)CIS.Foms.Enums.ContactTypes.Email)?.Value;
            }

            model.Add(c);
        }

        return model;
    }

    private readonly ILogger<GetCustomersHandler> _logger;
    private readonly DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSaService;
    private readonly ICustomerServiceClient _customerService;

    public GetCustomersHandler(
        ILogger<GetCustomersHandler> logger,
        ICustomerServiceClient customerService,
        DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient customerOnSaService)
    {
        _customerService = customerService;
        _customerOnSaService = customerOnSaService;
        _logger = logger;
    }
}