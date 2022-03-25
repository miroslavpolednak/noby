using DomainServices.CustomerService.Abstraction;
using CIS.Infrastructure.gRPC.CisTypes;
using _Customer = DomainServices.CustomerService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA.Shared;

[CIS.Infrastructure.Attributes.TransientService, CIS.Infrastructure.Attributes.SelfService]
internal class IdentifyCustomerService
{
    public async Task<(int? PartnerId, List<Identity>? Identities)> FillEntity(Repositories.Entities.CustomerOnSA entity, Contracts.CustomerOnSABase? customer, CancellationToken cancellation)
    {
        int? newMpIdentityId = null;

        // pokud se jedna o existujici identitu v KB
        if (customer?.CustomerIdentifiers is not null && customer.CustomerIdentifiers.Any())
        {
            var ident = customer.CustomerIdentifiers.First();

            //TODO nepotrebuju asi cely detail, tak by stacila nejaka metoda CustomerExists? Nebo mam propsat udaje z CM nize do CustomerOnSA?
            // pokud klient neexistuje, mela by CustomerService vyhodit vyjimku
            var customerInstance = ServiceCallResult.Resolve<_Customer.CustomerResponse>(await _customerService.GetCustomerDetail(new() { Identity = ident }, cancellation));

            // propsat udaje do customerOnSA
            entity.DateOfBirthNaturalPerson = customerInstance.NaturalPerson?.DateOfBirth;
            entity.FirstNameNaturalPerson = customerInstance.NaturalPerson?.FirstName;
            entity.Name = customerInstance.NaturalPerson?.LastName ?? "";
            if (entity.Identities is null || !entity.Identities.Any(t => (int)t.IdentityScheme == (int)ident.IdentityScheme))
                entity.Identities = new() { new Repositories.Entities.CustomerOnSAIdentity(ident) };

            // pokud jeste nema modre ID, ale ma cervene
            if (!customer.CustomerIdentifiers.Any(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Mp))
            {
                //TODO jak tady bude vypadat volani pro PO?
                // zavolat EAS
                newMpIdentityId = resolveCreateEasClient(await _easClient.CreateNewOrGetExisingClient(new()
                {
                    BirthNumber = customerInstance.NaturalPerson!.BirthNumber,
                    FirstName = customerInstance.NaturalPerson.FirstName,
                    LastName = customerInstance.NaturalPerson.LastName,
                    DateOfBirth = customerInstance.NaturalPerson.DateOfBirth
                }));

                // pokud se to povedlo, pridej customerovi modrou identitu
                if (newMpIdentityId.HasValue)
                    entity.Identities!.Add(new()
                    {
                        IdentityScheme = CIS.Foms.Enums.IdentitySchemes.Mp,
                        CustomerOnSAIdentityId = newMpIdentityId.Value
                    });
            }
        }
        else // neexistujici customer
        {
            entity.DateOfBirthNaturalPerson = customer?.DateOfBirthNaturalPerson;
            entity.FirstNameNaturalPerson = customer?.FirstNameNaturalPerson ?? "";
            entity.Name = customer?.Name ?? "";
        }

        return (newMpIdentityId, entity.Identities?.Select(t => new Identity(t.CustomerOnSAIdentityId, t.IdentityScheme)).ToList());
    }

    // zalozit noveho klienta v EAS
    static int? resolveCreateEasClient(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<ExternalServices.Eas.Dto.CreateNewOrGetExisingClientResponse> r => r.Model.Id,
            ErrorServiceCallResult r => default(int?), //TODO co se ma v tomhle pripade delat?
            _ => throw new NotImplementedException("resolveCreateEasClient")
        };

    private readonly ICustomerServiceAbstraction _customerService;
    private readonly Eas.IEasClient _easClient;

    public IdentifyCustomerService(
        ICustomerServiceAbstraction customerService,
        Eas.IEasClient easClient)
    {
        _customerService = customerService;
        _easClient = easClient;
    }
}
