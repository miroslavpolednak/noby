using DomainServices.CustomerService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.SalesArrangementService.Contracts;
using FOMS.Services.CreateCustomer;
using _Customer = DomainServices.CustomerService.Contracts;

namespace FOMS.Services;

[CIS.Infrastructure.Attributes.TransientService, CIS.Infrastructure.Attributes.SelfService]
public sealed class CreateCustomerService
{
    /// <summary>
    /// Vytvoreni noveho CustomerOnSA.
    /// Pokud je klient identifikovany, dotahnou se jeho udaje z CM a propisou do CustomerOnSA.
    /// Pokud klient neni identifikovany, pouziji se jako zakladni identifikator udaje z FE - requestu
    /// </summary>
    public async Task<(int CustomerOnSAId, int? PartnerId)> Create(int salesArrangementId, IClientInfo request, CancellationToken cancellationToken)
    {
        CreateCustomerRequest createCustomerRequest;
        int? partnerId = default(int?);

        if (request.Identity is null || request.Identity?.Id == 0) // neidentifikovany klient
        {
            createCustomerRequest = createRequest(request, salesArrangementId);
        }
        else // identifikovany klient
        {
            //TODO co delat, kdyz ho nenajdu?
            var customer = ServiceCallResult.Resolve<_Customer.CustomerResponse>(await _customerService.GetCustomerDetail(new DomainServices.CustomerService.Contracts.CustomerRequest
            {
                Identity = request.Identity!
            }, cancellationToken));

            createCustomerRequest = createRequest(customer, salesArrangementId);

            // vytvorit modre ID
            partnerId = resolveCreateClient(await _easClient.CreateNewOrGetExisingClient(new ExternalServices.Eas.Dto.ClientDataModel
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth
            }));
            if (partnerId.HasValue)
                createCustomerRequest.CustomerIdentifiers.Add(new CIS.Infrastructure.gRPC.CisTypes.Identity(partnerId.Value, CIS.Foms.Enums.IdentitySchemes.Mp));
        }

        // vytvorit customera
        var customerId = ServiceCallResult.Resolve<int>(await _customerOnSAService.CreateCustomer(createCustomerRequest, cancellationToken));

        return (customerId, partnerId);
    }

    // zalozit noveho klienta v EAS
    private static int? resolveCreateClient(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<ExternalServices.Eas.Dto.CreateNewOrGetExisingClientResponse> r => r.Model.Id,
            ErrorServiceCallResult r => default(int?), //TODO co se ma v tomhle pripade delat?
            _ => throw new NotImplementedException("CreateCustomerWithHouseholdService.resolveCreateClient")
        };

    /// <summary>
    /// Vytvoreni requestu pro identifikovaneho klienta
    /// </summary>
    private static CreateCustomerRequest createRequest(_Customer.CustomerResponse customer, int salesArrangementId)
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
    private static CreateCustomerRequest createRequest(IClientInfo request, int salesArrangementId)
        => new CreateCustomerRequest
        {
            SalesArrangementId = salesArrangementId,
            CustomerRoleId = (int)CIS.Foms.Enums.CustomerRoles.Debtor,
            DateOfBirthNaturalPerson = request.DateOfBirth,
            FirstNameNaturalPerson = request.FirstName,
            Name = request.LastName
        };

    private readonly Eas.IEasClient _easClient;
    private readonly ICustomerServiceAbstraction _customerService;
    private readonly ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ILogger<CreateCustomerService> _logger;

    public CreateCustomerService(
        Eas.IEasClient easClient,
        ILogger<CreateCustomerService> logger, 
        ICustomerOnSAServiceAbstraction customerOnSAService,
        IHouseholdServiceAbstraction householdService,
        ICustomerServiceAbstraction customerService)
    {
        _easClient = easClient;
        _logger = logger;
        _householdService = householdService;
        _customerService = customerService;
        _customerOnSAService = customerOnSAService;
    }
}
