using DomainServices.CustomerService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.SalesArrangementService.Contracts;

namespace FOMS.Api.Endpoints.Offer.CreateMortgageCase;

[CIS.Infrastructure.Attributes.TransientService, CIS.Infrastructure.Attributes.SelfService]
internal class CreateCustomerWithHouseholdService
{
    public async Task<(int HouseholdId, int CustomerOnSAId, int? PartnerId)> Create(int salesArrangementId, CreateMortgageCaseRequest request, CancellationToken cancellationToken)
    {
        // nejdriv vytvorim klienta
        var customerResult = await createCustomer(salesArrangementId, request, cancellationToken);

        // pak vytvorim domacnost a rovnou ji navazu na pred tim vytvoreneho klienta
        int householdId = ServiceCallResult.Resolve<int>(await _householdService.CreateHousehold(new CreateHouseholdRequest
        {
            HouseholdTypeId = (int)CIS.Foms.Enums.HouseholdTypes.Debtor,
            SalesArrangementId = salesArrangementId,
            CustomerOnSAId1 = customerResult.CustomerOnSAId
        }, cancellationToken));

        return (householdId, customerResult.CustomerOnSAId, customerResult.PartnerId);
    }

    /// <summary>
    /// Vytvoreni noveho CustomerOnSA.
    /// Pokud je klient identifikovany, dotahnou se jeho udaje z CM a propisou do CustomerOnSA.
    /// Pokud klient neni identifikovany, pouziji se jako zakladni identifikator udaje z FE - requestu
    /// </summary>
    private async Task<(int CustomerOnSAId, int? PartnerId)> createCustomer(int salesArrangementId, CreateMortgageCaseRequest request, CancellationToken cancellationToken)
    {
        CreateCustomerRequest createCustomerRequest;
        int? partnerId = default(int?);

        if (request.Customer is null || request.Customer.Id == 0) // neidentifikovany klient
        {
            createCustomerRequest = createRequest(request, salesArrangementId);
        }
        else // identifikovany klient
        {
            //TODO co delat, kdyz ho nenajdu?
            var customer = ServiceCallResult.Resolve<DomainServices.CustomerService.Contracts.CustomerResponse>(await _customerService.GetCustomerDetail(new DomainServices.CustomerService.Contracts.CustomerRequest
            {
                Identity = request.Customer
            }, cancellationToken));

            createCustomerRequest = createRequest(customer, salesArrangementId);

            // vytvorit modre ID
            partnerId = 1;
            createCustomerRequest.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(partnerId.Value, CIS.Foms.Enums.IdentitySchemes.Mp));
        }

        // vytvorit customera
        var customerId = ServiceCallResult.Resolve<int>(await _customerOnSAService.CreateCustomer(createCustomerRequest, cancellationToken));

        return (customerId, partnerId);
    }

    /// <summary>
    /// Vytvoreni requestu pro identifikovaneho klienta
    /// </summary>
    private static CreateCustomerRequest createRequest(DomainServices.CustomerService.Contracts.CustomerResponse customer, int salesArrangementId)
    {
        var model = new CreateCustomerRequest
        {
            SalesArrangementId = salesArrangementId,
            CustomerRoleId = (int)CIS.Foms.Enums.CustomerRoles.Debtor,
            DateOfBirthNaturalPerson = customer.NaturalPerson?.DateOfBirth,
            FirstNameNaturalPerson = customer.NaturalPerson?.FirstName,
            Name = customer.NaturalPerson?.LastName
        };
        model.CustomerIdentifiers.AddRange(customer.Identities);

        return model;
    }

    /// <summary>
    /// Vytvoreni requestu pro zalozeni neidentifikovaneho klienta
    /// </summary>
    private static CreateCustomerRequest createRequest(CreateMortgageCaseRequest request, int salesArrangementId)
        => new CreateCustomerRequest
        {
            SalesArrangementId = salesArrangementId,
            CustomerRoleId = (int)CIS.Foms.Enums.CustomerRoles.Debtor,
            DateOfBirthNaturalPerson = request.DateOfBirth,
            FirstNameNaturalPerson = request.FirstName,
            Name = request.LastName
        };

    private readonly ICustomerServiceAbstraction _customerService;
    private readonly ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ILogger<CreateCustomerWithHouseholdService> _logger;

    public CreateCustomerWithHouseholdService(
        ILogger<CreateCustomerWithHouseholdService> logger, 
        ICustomerOnSAServiceAbstraction customerOnSAService,
        IHouseholdServiceAbstraction householdService,
        ICustomerServiceAbstraction customerService)
    {
        _logger = logger;
        _householdService = householdService;
        _customerService = customerService;
        _customerOnSAService = customerOnSAService;
    }
}
