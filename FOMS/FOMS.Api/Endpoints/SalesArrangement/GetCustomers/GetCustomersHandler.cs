﻿using DomainServices.CustomerService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetCustomers;

internal class GetCustomersHandler
    : IRequestHandler<GetCustomersRequest, List<Dto.CustomerListItem>>
{
    //TODO tohle se bude nejspis cele predelavat, nema smysl to moc resit
    public async Task<List<Dto.CustomerListItem>> Handle(GetCustomersRequest request, CancellationToken cancellationToken)
    {
        _logger.RequestHandlerStartedWithId(nameof(GetCustomersHandler), request.SalesArrangementId);

        // najit existujici customeryOnSA
        var customersOnSA = ServiceCallResult.Resolve<List<DomainServices.SalesArrangementService.Contracts.CustomerOnSA>>(await _customerOnSaService.GetCustomerList(request.SalesArrangementId, cancellationToken));

        _logger.FoundItems(customersOnSA.Count);
        
        List<Dto.CustomerListItem> model = new();
        
        //TODO idealne natahnout z customerService vsechny najednou?
        customersOnSA.ForEach(async t =>
        {
            var c = new Dto.CustomerListItem()
            {
                Id = t.CustomerOnSAId,
                FirstName = t.FirstNameNaturalPerson,
                LastName = t.Name,
                DateOfBirth = t.DateOfBirthNaturalPerson
            };
            
            // zavolat customer svc pro detail
            //TODO nejak prioritizovat schemata?
            var identity = new CIS.Infrastructure.gRPC.CisTypes.Identity
            {
                IdentityId = t.CustomerIdentifiers[0].IdentityId,
                IdentityScheme = t.CustomerIdentifiers[0].IdentityScheme
            };
            var customerDetail = ServiceCallResult.Resolve<DomainServices.CustomerService.Contracts.CustomerResponse>(await _customerService.GetCustomerDetail(new CustomerRequest() {Identity = identity}));

            // adresa
            //TODO kterou adresu brat?
            var address = customerDetail.Addresses?.FirstOrDefault();
            if (address is not null)
            {
                c.City = address.City;
                c.Street = address.Street;
            }

            // kontakty
            //TODO jak poznam jake kontakty se maji naplnit?
            c.Phone = customerDetail.Contacts?.FirstOrDefault(x => x.ContactTypeId == 1)?.Value;
            c.Email = customerDetail.Contacts?.FirstOrDefault(x => x.ContactTypeId == 2)?.Value;

            model.Add(c);
        });

        return model;
    }

    private readonly ILogger<GetCustomersHandler> _logger;
    private readonly DomainServices.SalesArrangementService.Abstraction.ICustomerOnSAServiceAbstraction _customerOnSaService;
    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public GetCustomersHandler(
        ILogger<GetCustomersHandler> logger,
        DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService,
        DomainServices.SalesArrangementService.Abstraction.ICustomerOnSAServiceAbstraction customerOnSaService)
    {
        _customerService = customerService;
        _customerOnSaService = customerOnSaService;
        _logger = logger;
    }
}