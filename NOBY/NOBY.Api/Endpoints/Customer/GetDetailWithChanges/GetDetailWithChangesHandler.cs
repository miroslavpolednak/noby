using __SA = DomainServices.SalesArrangementService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using CIS.Infrastructure.gRPC.CisTypes;
using NOBY.Api.SharedDto;

namespace NOBY.Api.Endpoints.Customer.GetDetailWithChanges;

internal sealed class GetDetailWithChangesHandler
    : IRequestHandler<GetDetailWithChangesRequest, GetDetailWithChangesResponse>
{
    public async Task<GetDetailWithChangesResponse> Handle(GetDetailWithChangesRequest request, CancellationToken cancellationToken)
    {
        // customer instance
        var customerOnSA = await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken);

        // kontrola identity KB
        var kbIdentity = customerOnSA.CustomerIdentifiers
            .FirstOrDefault(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Kb)
            ?? throw new CisValidationException("Customer is missing KB identity");

        // SA instance
        var salesArrangement = ServiceCallResult.ResolveAndThrowIfError<__SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(customerOnSA.SalesArrangementId, cancellationToken));

        // kontrola mandanta
        var productTypeId = (await _codebookService.SalesArrangementTypes(cancellationToken)).First(t => t.Id == salesArrangement.SalesArrangementTypeId).ProductTypeId;
        // mandant produktu
        var productMandant = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == productTypeId).MandantId;
        if (productMandant != 2) // muze byt jen KB
            throw new CisValidationException("Product type mandant is not KB");

        // instance customer z KB CM
        var customer = ServiceCallResult.ResolveAndThrowIfError<DomainServices.CustomerService.Contracts.CustomerDetailResponse>(await _customerService.GetCustomerDetail(kbIdentity, cancellationToken));

        Dto.NaturalPerson person = new();
        customer.NaturalPerson?.FillResponseDto(person);
        person.EducationLevelId = customer.NaturalPerson?.EducationLevelId;
        //person.ProfessionCategoryId = customer.NaturalPerson?
        //person.ProfessionId = customer.NaturalPerson ?;
        //person.NetMonthEarningAmountId = customer.NaturalPerson
        //person.NetMonthEarningTypeId = customer.NaturalPerson ?;

        return new GetDetailWithChangesResponse
        {
            NaturalPerson = person,
            JuridicalPerson = null,
            IdentificationDocument = customer.IdentificationDocument?.ToResponseDto(),
            Contacts = customer.Contacts?.ToResponseDto(),
            Addresses = customer.Addresses?.Select(t => (CIS.Foms.Types.Address)t!).ToList()
        };
    }

    private readonly ICodebookServiceClients _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICustomerServiceClient _customerService;

    public GetDetailWithChangesHandler(
        ICustomerServiceClient customerService,
        ICodebookServiceClients codebookService,
        ISalesArrangementServiceClient salesArrangementService,
        ICustomerOnSAServiceClient customerOnSAService)
    {
        _customerService = customerService;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _customerOnSAService = customerOnSAService;
    }
}
