﻿using DomainServices.SalesArrangementService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.CodebookService.Clients;

namespace NOBY.Api.Endpoints.Customer.GetDetailWithChanges;

internal sealed class GetDetailWithChangesHandler
    : IRequestHandler<GetDetailWithChangesRequest, GetDetailWithChangesResponse>
{
    public async Task<GetDetailWithChangesResponse> Handle(GetDetailWithChangesRequest request, CancellationToken cancellationToken)
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

        return await _changedDataService.GetCustomerWithChangedData<GetDetailWithChangesResponse>(customerOnSA, cancellationToken);
    }

    private readonly CustomerWithChangedDataService _changedDataService;
    private readonly ICodebookServiceClients _codebookService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;

    public GetDetailWithChangesHandler(
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
