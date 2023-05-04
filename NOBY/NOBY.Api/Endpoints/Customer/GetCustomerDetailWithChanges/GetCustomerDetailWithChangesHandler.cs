using DomainServices.SalesArrangementService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.CodebookService.Clients;

namespace NOBY.Api.Endpoints.Customer.GetCustomerDetailWithChanges;

internal sealed class GetCustomerDetailWithChangesHandler
    : IRequestHandler<GetCustomerDetailWithChangesRequest, GetCustomerDetailWithChangesResponse>
{
    public async Task<GetCustomerDetailWithChangesResponse> Handle(GetCustomerDetailWithChangesRequest request, CancellationToken cancellationToken)
    {
        // customer instance
        var customerOnSA = await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken);

        // SA instance
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(customerOnSA.SalesArrangementId, cancellationToken);

        // kontrola mandanta
        var productTypeId = (await _codebookService.SalesArrangementTypes(cancellationToken)).First(t => t.Id == salesArrangement.SalesArrangementTypeId).ProductTypeId;
        // mandant produktu
        var productMandant = (await _codebookService.ProductTypes(cancellationToken)).First(t => t.Id == productTypeId).MandantId;
        if (productMandant != 2) // muze byt jen KB
            throw new CisValidationException("Product type mandant is not KB");

        var (data, _) = await _changedDataService.GetCustomerWithChangedData<GetCustomerDetailWithChangesResponse>(customerOnSA, cancellationToken);
        return data;
    }

    private readonly CustomerWithChangedDataService _changedDataService;
    private readonly ICodebookServiceClients _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;

    public GetCustomerDetailWithChangesHandler(
        CustomerWithChangedDataService changedDataService,
        ICodebookServiceClients codebookService,
        ISalesArrangementServiceClient salesArrangementService,
        ICustomerOnSAServiceClient customerOnSAService)
    {
        _changedDataService = changedDataService;
        _codebookService = codebookService;
        _salesArrangementService = salesArrangementService;
        _customerOnSAService = customerOnSAService;
    }
}
