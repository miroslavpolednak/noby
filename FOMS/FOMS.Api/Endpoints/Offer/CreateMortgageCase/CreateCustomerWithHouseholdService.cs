using DomainServices.CustomerService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;

namespace FOMS.Api.Endpoints.Offer.CreateMortgageCase;

[CIS.Infrastructure.Attributes.TransientService, CIS.Infrastructure.Attributes.SelfService]
internal class CreateCustomerWithHouseholdService
{
    public async Task<(int HouseholdId, int CustomerOnSAId)> Create(int salesArrangementId, CreateMortgageCaseRequest request, CancellationToken cancellationToken)
    {
        int customerId = await createCustomer(salesArrangementId, request, cancellationToken);

        int householdId = ServiceCallResult.Resolve<int>(await _householdService.CreateHousehold(new DomainServices.SalesArrangementService.Contracts.CreateHouseholdRequest
        {
            HouseholdTypeId = (int)CIS.Foms.Enums.HouseholdTypes.Debtor,
            SalesArrangementId = salesArrangementId,
            CustomerOnSAId1 = customerId
        }, cancellationToken));

        return (customerId, householdId);
    }

    private async Task<int> createCustomer(int salesArrangementId, CreateMortgageCaseRequest request, CancellationToken cancellationToken)
    {
        DomainServices.SalesArrangementService.Contracts.CreateCustomerRequest createCustomerRequest = new()
        {
            SalesArrangementId = salesArrangementId,
            CustomerRoleId = (int)CIS.Foms.Enums.CustomerRoles.Debtor,
        };

        if (request.Customer is null || request.Customer.Id == 0) // neidentifikovany klient
        {
            createCustomerRequest = new DomainServices.SalesArrangementService.Contracts.CreateCustomerRequest
            {
                SalesArrangementId = salesArrangementId,
                CustomerRoleId = (int)CIS.Foms.Enums.CustomerRoles.Debtor,
                DateOfBirthNaturalPerson = request.DateOfBirth,
                FirstNameNaturalPerson = request.FirstName,
                Name = request.LastName
            };
        }
        else // identifikovany klient
        {
            //TODO co delat, kdyz ho nenajdu?
            var customer = ServiceCallResult.Resolve<DomainServices.CustomerService.Contracts.CustomerResponse>(await _customerService.GetCustomerDetail(new DomainServices.CustomerService.Contracts.CustomerRequest
            {
                Identity = request.Customer
            }, cancellationToken));

            createCustomerRequest = new DomainServices.SalesArrangementService.Contracts.CreateCustomerRequest
            {
                SalesArrangementId = salesArrangementId,
                CustomerRoleId = (int)CIS.Foms.Enums.CustomerRoles.Debtor,
                DateOfBirthNaturalPerson = customer.NaturalPerson?.DateOfBirth,
                FirstNameNaturalPerson = customer.NaturalPerson?.FirstName,
                Name = customer.NaturalPerson?.LastName
            };
            createCustomerRequest.CustomerIdentifiers.AddRange(customer.Identities);
        }

        // vytvorit customera
        return ServiceCallResult.Resolve<int>(await _customerOnSAService.CreateCustomer(createCustomerRequest, cancellationToken));
    }

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
